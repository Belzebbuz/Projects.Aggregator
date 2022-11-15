using System.Net;

namespace SharedLibrary.Exceptions.EF;

public class NotLoadedException : CustomException
{
    public NotLoadedException(string property, string entity, string id)
        : base($"Navigation property {property} was not loaded in entity {entity}, Entity Id: {id}", null, HttpStatusCode.InternalServerError)
    {
    }
}

public class ReleaseNotSavedException : CustomException
{
    public ReleaseNotSavedException()
        : base("Release must be saved before adding to database", null, HttpStatusCode.InternalServerError)
    {
    }
}