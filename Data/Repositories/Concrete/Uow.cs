using Data.Contexts;
using Data.Repositories.Interface;

namespace Data.Repositories.Concrete;

public class Uow : IUow
{
    private readonly BloggerAppDbContext _context;
    private readonly WebInfoRepository _webInfoRepository = null!;
    private readonly SystemLogRepository _systemLogRepository = null!;
    private readonly FolderRepository _folderRepository = null!;
    private readonly ResumeRepository _resumeRepository = null!;
    private readonly ProjectRepository _projectRepository = null!;
    
    public Uow(BloggerAppDbContext context)
    {
        _context = context;
    }

    public IWebInfoRepository WebInfoRepository => _webInfoRepository ?? new WebInfoRepository(_context);
    public ISystemLogRepository SystemLogRepository => _systemLogRepository ?? new SystemLogRepository(_context);
    public IFolderRepository FolderRepository => _folderRepository ?? new FolderRepository(_context);
    public IResumeRepository ResumeRepository => _resumeRepository ?? new ResumeRepository(_context);
    public IProjectRepository ProjectRepository => _projectRepository ?? new ProjectRepository(_context);

    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
    }
}