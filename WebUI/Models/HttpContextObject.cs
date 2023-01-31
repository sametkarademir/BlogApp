namespace WebUI.Models;

public class HttpContextObject
{
    public string? Username { get; set; }
    public string? UserId { get; set; }
    public string? ControllerName { get; set; }
    public string? ActionName { get; set; }
    public string? RemoteAddress { get; set; }
    public string? RemotePort { get; set; }
}