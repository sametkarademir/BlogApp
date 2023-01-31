namespace Entities.Concrete;

public class SystemLog 
{
    public string Id { get; set; } = null!;
    public string? Message { get; set; }
    public string? Username { get; set; }
    public DateTime? Date { get; set; }
    public string? RemoteAddress { get; set; }
    public string? RemotePort { get; set; }
    public string? Action { get; set; }
    public string? Method { get; set; }
    public LogStatus LogStatus { get; set; }
}

public enum LogStatus
{
    Success = 0,
    Info = 1,
    Warning = 2,
    Error = 3,
}
