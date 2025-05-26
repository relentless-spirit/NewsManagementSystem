using Microsoft.EntityFrameworkCore;
using NewsManagementSystem.DAL.DBContext;
using NewsManagementSystem.DAL.Entities;

namespace NewsManagementSystem.DAL.Repositories.Category;

public class CategoryRepo : ICategoryRepo
{
    private readonly FUNewsManagementContext _context;

    public CategoryRepo(FUNewsManagementContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<List<Entities.Category>> GetCategoriesAsync()
    {
        return await _context.Categories.ToListAsync();
    }

    public Task<Entities.Category?> GetCategoryByNameAsync(string categoryName)
    {
        throw new NotImplementedException();
    }

    public Task CreateCategoryAsync(Entities.Category category)
    {
        throw new NotImplementedException();
    }

    public Task UpdateCategoryAsync(Entities.Category category)
    {
        throw new NotImplementedException();
    }

    public Task DeleteCategoryAsync(Entities.Category category)
    {
        throw new NotImplementedException();
    }
}