using eCommerce.Core.Entities;

namespace eCommerce.Core.Specifications
{
    public class CategoriesWithProductsSpecification : BaseSpecification<Category>
    {
        public CategoriesWithProductsSpecification()
        {
            AddInclude(x => x.Products);
        }

        public CategoriesWithProductsSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude(x => x.Products);
        }
    }
}
