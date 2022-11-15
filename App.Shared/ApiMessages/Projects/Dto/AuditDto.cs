namespace App.Shared.ApiMessages.Projects.Dto;

public class AuditDto
{
    public Guid Id { get; set; }
    public string CreatedByEmail { get; set; }
    public string LastModifiedOn { get; set; }
}
