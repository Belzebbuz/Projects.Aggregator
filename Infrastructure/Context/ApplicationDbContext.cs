using Application.Contracts.Identity;
using Application.Contracts.Services;
using Application.Contracts.Services.Events;
using Domain.Aggregators.BugAggregate;
using Domain.Aggregators.ProjectAggregate;
using Domain.Base;
using Infrastructure.Auditing;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SharedLibrary.Wrapper;

namespace Infrastructure.Context;

public class ApplicationDbContext : IdentityDbContext<AppUser>
{
    private readonly DatabaseSettings _dbSettings;
    private readonly ISerializerService _serializer;
    protected readonly ICurrentUser _currentUser;
    private readonly IEventPublisher _events;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
        IOptions<DatabaseSettings> dbSettings,
        ISerializerService serializer,
        ICurrentUser currentUser,
        IEventPublisher events)
        : base(options)
    {
        _dbSettings = dbSettings.Value;
        _serializer = serializer;
        _currentUser = currentUser;
        _events = events;
    }

    public DbSet<Trail> AuditTrails => Set<Trail>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Release> Releases => Set<Release>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<BugReport> BugReports => Set<BugReport>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.AppendGlobalQueryFilter<ISoftDelete>(s => s.DeletedOn == null);
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseDatabase(_dbSettings.Provider!, _dbSettings.ConnectionString!);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var auditEntries = HandleAuditingBeforeSaveChanges(_currentUser.GetUserId(), _currentUser.GetUserEmail());

        int result = await base.SaveChangesAsync(cancellationToken);

        await HandleAuditingAfterSaveChangesAsync(auditEntries, cancellationToken);

        await SendDomainEventsAsync();

        return result;
    }
    private List<AuditTrail> HandleAuditingBeforeSaveChanges(Guid userId, string email)
    {
        foreach (var entry in ChangeTracker.Entries<IAuditableEntity>().ToList())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = userId;
                    entry.Entity.CreatedByEmail = email;
                    entry.Entity.LastModifiedBy = userId;
                    entry.Entity.LastModifiedByEmail = email;
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModifiedOn = DateTime.UtcNow.ConvertToMoscowTime();
                    entry.Entity.LastModifiedBy = userId;
                    entry.Entity.LastModifiedByEmail = email;
                    break;

                case EntityState.Deleted:
                    if (entry.Entity is ISoftDelete softDelete)
                    {
                        softDelete.DeletedBy = userId;
                        softDelete.DeletedOn = DateTime.UtcNow.ConvertToMoscowTime();
                        entry.State = EntityState.Modified;
                    }

                    break;
            }
        }

        ChangeTracker.DetectChanges();

        var trailEntries = new List<AuditTrail>();
        foreach (var entry in ChangeTracker.Entries<IAuditableEntity>()
                     .Where(e => e.State is EntityState.Added or EntityState.Deleted or EntityState.Modified)
                     .ToList())
        {
            var trailEntry = new AuditTrail(entry, _serializer)
            {
                TableName = entry.Entity.GetType().Name,
                UserId = userId
            };
            trailEntries.Add(trailEntry);
            foreach (var property in entry.Properties)
            {
                if (property.IsTemporary)
                {
                    trailEntry.TemporaryProperties.Add(property);
                    continue;
                }

                string propertyName = property.Metadata.Name;
                if (property.Metadata.IsPrimaryKey())
                {
                    trailEntry.KeyValues[propertyName] = property.CurrentValue;
                    continue;
                }

                switch (entry.State)
                {
                    case EntityState.Added:
                        trailEntry.TrailType = TrailType.Create;
                        trailEntry.NewValues[propertyName] = property.CurrentValue;
                        break;

                    case EntityState.Deleted:
                        trailEntry.TrailType = TrailType.Delete;
                        trailEntry.OldValues[propertyName] = property.OriginalValue;
                        break;

                    case EntityState.Modified:
                        if (property.IsModified && entry.Entity is ISoftDelete && property.OriginalValue == null && property.CurrentValue != null)
                        {
                            trailEntry.ChangedColumns.Add(propertyName);
                            trailEntry.TrailType = TrailType.Delete;
                            trailEntry.OldValues[propertyName] = property.OriginalValue;
                            trailEntry.NewValues[propertyName] = property.CurrentValue;
                        }
                        else if (property.IsModified && property.OriginalValue?.Equals(property.CurrentValue) == false)
                        {
                            trailEntry.ChangedColumns.Add(propertyName);
                            trailEntry.TrailType = TrailType.Update;
                            trailEntry.OldValues[propertyName] = property.OriginalValue;
                            trailEntry.NewValues[propertyName] = property.CurrentValue;
                        }

                        break;
                }
            }
        }

        foreach (var auditEntry in trailEntries.Where(e => !e.HasTemporaryProperties))
        {
            AuditTrails.Add(auditEntry.ToAuditTrail());
        }

        return trailEntries.Where(e => e.HasTemporaryProperties).ToList();
    }
    private Task HandleAuditingAfterSaveChangesAsync(List<AuditTrail> trailEntries, CancellationToken cancellationToken = new())
    {
        if (trailEntries == null || trailEntries.Count == 0)
        {
            return Task.CompletedTask;
        }

        foreach (var entry in trailEntries)
        {
            foreach (var prop in entry.TemporaryProperties)
            {
                if (prop.Metadata.IsPrimaryKey())
                {
                    entry.KeyValues[prop.Metadata.Name] = prop.CurrentValue;
                }
                else
                {
                    entry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
                }
            }

            AuditTrails.Add(entry.ToAuditTrail());
        }

        return SaveChangesAsync(cancellationToken);
    }
    private async Task SendDomainEventsAsync()
    {
        var entitiesWithEvents = ChangeTracker.Entries<IEntity>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents.Count > 0)
            .ToArray();

        foreach (var entity in entitiesWithEvents)
        {
            var domainEvents = entity.DomainEvents.ToArray();
            entity.DomainEvents.Clear();
            foreach (var domainEvent in domainEvents)
            {
                await _events.PublishAsync(domainEvent);
            }
        }
    }
}
