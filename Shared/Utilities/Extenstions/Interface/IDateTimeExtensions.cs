namespace Shared.Utilities.Extenstions.Interface;

public interface IDateTimeExtensions
{
    public DateTime Epoch { get; }
    public DateTime FromUnixTime(long milliseconds);
    public long ToUnixTime(DateTime date);
}
