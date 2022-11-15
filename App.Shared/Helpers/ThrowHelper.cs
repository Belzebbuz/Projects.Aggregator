using App.Shared.Exceptions;
using App.Shared.Exceptions.EF;
using App.Shared.Wrapper;

namespace App.Shared.Helpers;

public static class ThrowHelper
{
	public static void StringNotEmpty(string text, string v)
	{
		if(string.IsNullOrEmpty(text))
			throw new ArgumentNullException(nameof(text));
	}

	public static void FileNotExists(string url)
	{
		if(!File.Exists(url))
			throw new FileNotFoundException(url);
	}

	public static void NotFoundEntity(object value, string entityId, string entityName)
	{
		if(value == null)
			throw new EntityNotFoundException(entityId, entityName);
	}

	public static void NotLoadedProperty(object value, string propertyName, string entityName, string id)
	{
		if(value == null)
			throw new NotLoadedException(propertyName, entityName, id);
	}

	public static void NotNull(object value, string entityName)
	{
		if(value == null)
			throw new ArgumentNullException(entityName);
	}

	public static async Task<IResult<HttpResponseMessage>> TrySendRequest(Func<Task<HttpResponseMessage>> request)
	{
		try
		{
			return await Result<HttpResponseMessage>.SuccessAsync(await request());
		}
		catch (Exception ex)
		{
			return Result<HttpResponseMessage>.Fail(ex.Message);
		}
	}
}
