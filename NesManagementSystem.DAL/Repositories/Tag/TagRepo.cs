using Microsoft.EntityFrameworkCore;
using NewsManagementSystem.DAL.DBContext;


namespace NewsManagementSystem.DAL.Repositories.Tag;

public class TagRepo : ITagRepo
{
    private readonly FUNewsManagementContext _context;

    public TagRepo(FUNewsManagementContext context)
    {
        _context = context;
    }

    public async Task<List<Entities.Tag>> GetAllTagsAsync()
    {
        return await _context.Tags.ToListAsync();
    }

    public async Task<List<Entities.Tag>> GetTagsByIdsAsync(List<int> tagIds)
    {
        return await _context.Tags.Where(t => tagIds.Contains(t.TagID)).ToListAsync();
    }
}