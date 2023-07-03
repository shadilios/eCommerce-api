namespace eCommerce.Core.Entities
{
    public class Category : BaseEntity
    {
        public string Description { get; set; }
        public string Image { get; set; }

        // NAV
        public List<Product>? Products { get; set; }
    }
}
