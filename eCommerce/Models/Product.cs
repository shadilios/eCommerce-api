namespace eCommerce.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }

        // FK
        public int CategoryId { get; set; }

        // NAV
    }
}
