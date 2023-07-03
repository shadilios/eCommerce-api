using eCommerce.Core.Entities;
using eCommerce.Data;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Services.CategoriesService
{
    public class CategoriesService : ICategories
    {
        private readonly AppDbContext _context;

        public CategoriesService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IReadOnlyList<Category>> GetCategoriesList()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }
    }
}
