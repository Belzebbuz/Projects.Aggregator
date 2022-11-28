using Domain.Base;
using System.ComponentModel.DataAnnotations;

namespace Domain.Aggregators.ProjectAggregate;

public class Tag : BaseEntity, IAggregateRoot
{
    [MaxLength(20)]
    public string Value { get; private set; }

    private ICollection<Project> _releases;
    public IReadOnlyCollection<Project> Releases => _releases.ToList();
    private Tag(string value)
    {
        Value = value ?? throw new ArgumentNullException(nameof(value));
    }

    public static Tag Create(string value)
    {
        return new Tag(value);
    }

    public override string ToString() => Value;
}