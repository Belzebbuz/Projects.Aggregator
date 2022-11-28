using SharedLibrary.Exceptions;
using SharedLibrary.Exceptions.EF;
using SharedLibrary.Wrapper;

namespace SharedLibrary.Helpers;

public static class ThrowHelper
{
    /// <summary>
    /// If string is null or empty throw ex
    /// </summary>
    /// <param name="text"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void StringNotEmpty(string text)
    {
        if (string.IsNullOrEmpty(text))
            throw new ArgumentNullException(nameof(text));
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="url"></param>
    /// <exception cref="FileNotFoundException"></exception>
    public static void FileNotExists(string url)
    {
        if (!File.Exists(url))
            throw new FileNotFoundException(url);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="entityId"></param>
    /// <param name="entityName"></param>
    /// <exception cref="EntityNotFoundException"></exception>
    public static void NotFoundEntity(object? value, string entityId, string entityName)
    {
        if (value == null)
            throw new EntityNotFoundException(entityId, entityName);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="propertyName"></param>
    /// <param name="entityName"></param>
    /// <param name="id"></param>
    /// <exception cref="NotLoadedException"></exception>
    public static void NotLoadedProperty(object value, string propertyName, string entityName, string id)
    {
        if (value == null)
            throw new NotLoadedException(propertyName, entityName, id);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="entityName"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void NotNull(object? value, string entityName)
    {
        if (value == null)
            throw new ArgumentNullException(entityName);
    }
}
