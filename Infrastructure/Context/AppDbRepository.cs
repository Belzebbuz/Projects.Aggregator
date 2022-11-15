using Application.Contracts.Repository;
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Domain.Base;
using Mapster;

namespace Infrastructure.Context;

public class AppDbRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T>
    where T : class, IAggregateRoot
{
    public AppDbRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
    protected override IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> specification) =>
        ApplySpecification(specification, false)
            .ProjectToType<TResult>();
}
