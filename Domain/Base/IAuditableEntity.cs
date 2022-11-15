namespace Domain.Base;

public interface IAuditableEntity
{
    public string CreatedByEmail { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedOn { get; }
    public Guid LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
    public string LastModifiedByEmail { get; set; }
}