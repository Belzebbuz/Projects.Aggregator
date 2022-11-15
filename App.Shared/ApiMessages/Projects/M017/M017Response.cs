namespace App.Shared.ApiMessages.Projects.M017;

/// <summary>
/// File with content type
/// </summary>
/// <param name="stream"></param>
/// <param name="ContentType"></param>
public record M017Response(Stream FileStream, string ContentType);