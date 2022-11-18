namespace SharedLibrary.ApiMessages.Constants;

internal class ValidateErrorMessages
{
    internal const string NotEmpty = "Поле должно быть заполнено!";

    internal static string MustBeLessThan(int length)
    {
        return $"Максимум {length} символов.";
    }
}
