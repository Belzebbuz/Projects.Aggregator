namespace SharedLibrary.ApiMessages.Projects.P009;

/// <summary>
/// File with content type
/// </summary>
/// <param name="stream"></param>
/// <param name="ContentType"></param>
public record P009Response(Stream FileStream, string ContentType);