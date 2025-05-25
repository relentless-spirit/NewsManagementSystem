using Microsoft.EntityFrameworkCore;
using NewsManagementSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsManagementSystem.DAL.Repositories
{
    public class CategoryRepo : ICategoryRepo
    {
        private readonly FUNewsManagementContext _context;

        public CategoryRepo(FUNewsManagementContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task CreateCategoryAsync(Category category)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteCategoryAsync(int categoryId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateCategoryAsync(Category category)
        {
            throw new NotImplementedException();
        }
    }
}
