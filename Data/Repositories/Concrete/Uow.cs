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
    private readonly CategoryRepository _categoryRepository = null!;
    private readonly ArticleRepository _articleRepository = null!;
    
    public Uow(BloggerAppDbContext context)
    {
        _context = context;
    }

    public IWebInfoRepository WebInfoRepository => _webInfoRepository ?? new WebInfoRepository(_context);
    public ISystemLogRepository SystemLogRepository => _systemLogRepository ?? new SystemLogRepository(_context);
    public IFolderRepository FolderRepository => _folderRepository ?? new FolderRepository(_context);
    public IResumeRepository ResumeRepository => _resumeRepository ?? new ResumeRepository(_context);
    public IProjectRepository ProjectRepository => _projectRepository ?? new ProjectRepository(_context);
    public ICategoryRepository CategoryRepository => _categoryRepository ?? new CategoryRepository(_context);
    public IArticleRepository ArticleRepository => _articleRepository ?? new ArticleRepository(_context);

    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
    }
}