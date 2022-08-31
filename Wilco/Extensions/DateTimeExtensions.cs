namespace Wilco;

public static class DateTimeExtensions
{
    /// <summary>
    /// Determines if the current time is between a given start and end time.
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="iStart"></param>
    /// <param name="iEnd"></param>
    /// <returns>True or false</returns>
    public static bool IsBetween(this DateTime dt, int iStart, int iEnd)
    {
        var start = new TimeSpan(iStart, 0, 0);
        var end = new TimeSpan(iEnd, 0, 0);
        var now = dt.TimeOfDay;

        return start < end ? start <= now && now <= end : !(end < now && now < start);
    }
}