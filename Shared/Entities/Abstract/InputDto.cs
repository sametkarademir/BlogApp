using System.Linq.Expressions;

namespace Shared.Entities.Abstract;

public class InputDto<T>
{
    public T? Data { get; set; }
    public ServiceInputDto? ServiceInputDto { get; set; }
}

public class ServiceInputDto
{
    public string? UserId { get; set; }
    public string? Username { get; set; }
    public string? RemoteAddress { get; set; }
    public string? RemotePort { get; set; }
    public string? RemoteAction { get; set; }
    public string? RemoteController { get; set; }
    public Status? Status { get; set; }
}
