namespace NewsManagementSystem.BLL.Services.Category;

using DAL.Entities;

public interface ICategoryService
{
    Task<List<Category>> GetCategoriesAsync();
    Task<Category?> GetCategoryByNameAsync(string name);
    Task CreateCategoryAsync(Category category);
    Task UpdateCategoryAsync(Category category);
    Task DeleteCategoryAsync(Category category);
}