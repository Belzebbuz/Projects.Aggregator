using System.Net;

namespace SharedLibrary.Exceptions;

public class UnauthorizedException : CustomException
{
    public UnauthorizedException(string message)
        : base(message, null, HttpStatusCode.Unauthorized)
    {
    }
}
