namespace Data.Repositories.Interface;

public interface IUow : IAsyncDisposable
{
    IWebInfoRepository WebInfoRepository { get; }
    ISystemLogRepository SystemLogRepository { get; }
    IFolderRepository FolderRepository { get; }
    IResumeRepository ResumeRepository { get; }
    IProjectRepository ProjectRepository { get; }


    Task<int> SaveAsync();
}