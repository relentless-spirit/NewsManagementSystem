namespace NewsManagementSystem.DAL.Repositories.Category;
using Entities;

public interface ICategoryRepo
{
    Task<List<Category>> GetCategoriesAsync();   
    Task<Category?> GetCategoryByNameAsync(string categoryName);
    Task CreateCategoryAsync(Category category);
    Task UpdateCategoryAsync(Category category);
    Task DeleteCategoryAsync(Category category);
}