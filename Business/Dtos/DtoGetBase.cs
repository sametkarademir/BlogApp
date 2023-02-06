namespace Business.Dtos;

public abstract class DtoGetBase
{
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 9;
    public int TotalCount { get; set; }
    public int TotalPages => (int)Math.Ceiling(decimal.Divide(TotalCount, PageSize));
    public bool ShowPrevious => CurrentPage > 1;
    public bool ShowNext => CurrentPage < TotalPages;
}