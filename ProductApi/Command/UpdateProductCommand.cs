using MediatR;

namespace ProductApiAsync.Command
{
    public class UpdateProductCommand : IRequest<int>
    {
        public UpdateProductCommand(int id, string name, string description, decimal price, string imageName, IFormFile productImage)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            ImageUrl = imageName;
            ProductImage = productImage;
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public IFormFile ProductImage { get; set; }
    }
}
