using System.Net;

namespace App.Shared.Exceptions;

public class UnauthorizedException : CustomException
{
	public UnauthorizedException(string message)
		: base(message, null, HttpStatusCode.Unauthorized)
	{
	}
}
