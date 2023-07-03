using eCommerce.Core.Entities;
using System.Collections.Generic;

namespace eCommerce.Services.CategoriesService
{
    public interface ICategories
    {
        Task<Category> GetCategoryByIdAsync(int id);

        Task<IReadOnlyList<Category>> GetCategoriesList();

    }
}
