using Entities.Concrete;

namespace Business.Dtos;

public class SystemLogListDto
{
    public List<SystemLog>? SystemLogList { get; set; }
}

public class RequestCountListDto
{
    public IEnumerable<RequestCountDto>? RequestCountDtos { get; set; }
}

public class RequestCountDto
{
    public string? Key { get; set; }
    public int? Count { get; set; }
}

public class SystemLogAddDto
{
    public string? Message { get; set; }
    public string? Username { get; set; }
    public DateTime? Date { get; set; }
    public string? RemoteAddress { get; set; }
    public string? RemotePort { get; set; }
    public string? Action { get; set; }
    public string? Method { get; set; }
    public LogStatus LogStatus { get; set; }
}