namespace Data.Repositories.Interface;

public interface IUow : IAsyncDisposable
{
    IWebInfoRepository WebInfoRepository { get; }


    Task<int> SaveAsync();
}