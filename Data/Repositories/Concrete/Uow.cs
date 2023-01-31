using Data.Contexts;
using Data.Repositories.Interface;

namespace Data.Repositories.Concrete;

public class Uow : IUow
{
    private readonly BloggerAppDbContext _context;
    private readonly WebInfoRepository _webInfoRepository = null!;
    
    public Uow(BloggerAppDbContext context)
    {
        _context = context;
    }

    public IWebInfoRepository WebInfoRepository => _webInfoRepository ?? new WebInfoRepository(_context);

    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
    }
}