namespace Infrastructure.Security;

public class JwtSettings
{
    public string Key { get; set; }
    public int ExpirationInDays { get; set; }
}
