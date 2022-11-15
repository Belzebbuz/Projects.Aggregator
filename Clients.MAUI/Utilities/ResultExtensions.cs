using SharedLibrary.Wrapper;
namespace Clients.MAUI.Utilities;

public static class SnackBarExtensions
{
    public static void HandleResult(this ISnackbar snackbar, IResult result, Action onSuccess)
    {
        if (result.Succeeded)
        {
            onSuccess();
        }
        else if (result.Messages.Count > 0)
        {
            foreach (var message in result.Messages)
            {
                snackbar.Add(message, Severity.Error);
            }
        }
    }
	public static void HandleResult(this ISnackbar snackbar, IResult result)
	{
		if (result.Messages.Count > 0)
		{
			foreach (var message in result.Messages)
			{
				snackbar.Add(message, Severity.Error);
			}
		}
	}

	public static void HandleResult(this ISnackbar snackbar, IResult result, string messageOnSuccess)
	{
		if (result.Succeeded)
		{
			snackbar.Add(messageOnSuccess, Severity.Success);
		}
		else
		{
			if (result.Messages.Count > 0)
			{
				foreach (var message in result.Messages)
				{
					snackbar.Add(message, Severity.Error);
				}
			}
		}
	}

	public static T HandleResult<T>(this ISnackbar snackbar, IResult<T> result)
		where T : class
	{
		if (result.Succeeded)
		{
			return result.Data;
		}
		else 
		{
			if(result.Messages.Count > 0)
			{
				foreach (var message in result.Messages)
				{
					snackbar.Add(message, Severity.Error);
				}
			}
			else
			{
				snackbar.Add("Error while handling result");
			}
			return null;
		}
	}
	public static PaginatedResult<T> HandleResult<T>(this ISnackbar snackbar, PaginatedResult<T> result)
		where T : class
	{
		if (result.Succeeded)
		{
			return result;
		}
		else
		{
			if (result.Messages.Count > 0)
			{
				foreach (var message in result.Messages)
				{
					snackbar.Add(message, Severity.Error);
				}
			}
			else
			{
				snackbar.Add("Error while handling result");
			}
			return null;
		}
	}
}
