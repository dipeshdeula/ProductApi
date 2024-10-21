﻿namespace ProductApiAsync.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }

        public string ImageUrl { get; set; } = null!;
    }
}
