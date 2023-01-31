using Shared.Utilities.Extenstions.Interface;

namespace Shared.Utilities.Extenstions.Concrete;

public class DateTimeExtensions : IDateTimeExtensions
{
    public DateTime Epoch => new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    public DateTime FromUnixTime(long milliseconds) => Epoch.AddMilliseconds(milliseconds);
    public long ToUnixTime(DateTime date) => (long)(date - Epoch).TotalMilliseconds;
}
