using eCommerce.Core.Entities;

namespace eCommerce.Api.Dtos
{
    public class CategoryToReturnDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        public string Products { get; set; }
    }
}
