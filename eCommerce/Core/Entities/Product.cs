using eCommerce.Utils.CustomValidation;

namespace eCommerce.Core.Entities
{
    public class Product : BaseEntity
    {
        public string Description { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }

        [DoubleRange(Min = 0, Max = 1)]
        public double Discount { get; set; }

        // FK
        public int CategoryId { get; set; }

        // NAV
    }
}
