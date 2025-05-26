using NewsManagementSystem.DAL.Repositories.Category;

namespace NewsManagementSystem.BLL.Services.Category;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepo _categoryRepo;

    public CategoryService(ICategoryRepo categoryRepo)
    {
        _categoryRepo = categoryRepo;
    }


    public async Task<List<DAL.Entities.Category>> GetCategoriesAsync()
    {
        return await _categoryRepo.GetCategoriesAsync();
    }

    public async Task<DAL.Entities.Category?> GetCategoryByNameAsync(string name)
    {
        throw new NotImplementedException();
    }

    public async Task CreateCategoryAsync(DAL.Entities.Category category)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateCategoryAsync(DAL.Entities.Category category)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteCategoryAsync(DAL.Entities.Category category)
    {
        throw new NotImplementedException();
    }
}