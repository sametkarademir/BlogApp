namespace Shared.Utilities.Result;

public class OperationResult<T> where T : new()
{
    public T? Data { get; set; }
    public ExeptionStatus ExeptionStatus { get; set; }
    public Exception? Exception { get; set; }
    public string? Message { get; set; }

    public OperationResult() { }
    public OperationResult(T? data, ExeptionStatus exeptionStatus, string message)
    {
        this.Data = data;
        this.ExeptionStatus = exeptionStatus;
        this.Message = message;
    }
}

public enum ExeptionStatus
{
    Success = 0,
    Error = 1,
    InvalidRequest = 2,
    ValidationError = 3,
    AccessDenied = 4,
}