using Microsoft.AspNetCore.Identity;
using SharedLibrary.Wrapper;

namespace Infrastructure.Identity;

public class AppUser : IdentityUser
{
    public string FirstName { get; set; }
    public string? LastName { get; set; }
    public bool IsActive { get; set; }
    public string? ImageUrl { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
    private AppUser()
    {
    }

    public static AppUser Create(string email, string? userName, string firstName, string? lastName, string? phoneNumber)
    {
        if (email == null) throw new ArgumentNullException(nameof(email));
        if (firstName == null) throw new ArgumentNullException(nameof(firstName));
        return new()
        {
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            PhoneNumber = phoneNumber,
            UserName = userName ?? email,
            IsActive = true
        };
    }

    public IResult SetImageUrl(string imageUrl)
    {
        ImageUrl = imageUrl;
        return Result.Success();
    }

    public IResult ToogleUserStatus(bool isActive)
    {
        IsActive = isActive;
        return Result.Success();
    }
}
