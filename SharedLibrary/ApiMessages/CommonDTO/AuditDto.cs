namespace SharedLibrary.ApiMessages.CommonDTO;

public class AuditDto
{
    public Guid Id { get; set; }
    public string CreatedByEmail { get; set; }
    public DateTime LastModifiedOn { get; set; }
    public string LastModifiedByEmail { get; set; }
}
