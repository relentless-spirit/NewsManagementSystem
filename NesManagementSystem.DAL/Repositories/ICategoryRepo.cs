using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewsManagementSystem.DAL.Repositories
{
    public interface ICategoryRepo
    {
        Task<List<Entities.Category>> GetAllCategoriesAsync();
        Task CreateCategoryAsync(Entities.Category category);
        Task UpdateCategoryAsync(Entities.Category category);
        Task DeleteCategoryAsync(int categoryId);
    }
}
