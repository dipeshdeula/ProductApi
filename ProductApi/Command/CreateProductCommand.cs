using MediatR;
using ProductApi.Models;

namespace ProductApiAsync.Command
{
    public class CreateProductCommand:IRequest<Product>
    {
        public CreateProductCommand(string name, string description, decimal price , string ImageName, IFormFile productImage)
        {
            Name = name;
            Description = description;
            Price = price;
            ImageUrl = ImageName;
            ProductImage = productImage;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public IFormFile ProductImage { get; set; }
    }
}
