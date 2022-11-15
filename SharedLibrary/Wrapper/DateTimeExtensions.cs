namespace SharedLibrary.Wrapper;

public static class DateTimeExtensions
{
    public static DateTime ConvertToMoscowTime(this DateTime dateTime)
    {
        TimeZoneInfo moscowZone = TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time");
        return TimeZoneInfo.ConvertTime(dateTime.ToUniversalTime(), moscowZone);
    }
}
