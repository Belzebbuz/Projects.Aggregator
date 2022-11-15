using System.Net;

namespace SharedLibrary.Exceptions;

public class EntityNotFoundException : CustomException
{
    public EntityNotFoundException(string id, string entityName)
        : base($"Entity: \"{entityName}\" with id: \"{id}\" not found", null, HttpStatusCode.NotFound)
    {
    }
}
