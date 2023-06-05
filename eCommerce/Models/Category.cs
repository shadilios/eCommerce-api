﻿namespace eCommerce.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        // NAV
        public List<Product>? Products { get; set; }
    }
}