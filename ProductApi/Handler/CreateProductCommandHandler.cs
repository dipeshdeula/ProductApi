using MediatR;
using ProductApi.Models;
using ProductApi.Services;
using ProductApiAsync.Command;

namespace ProductApiAsync.Handler
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Product>
    {
        private readonly IProductService _productService;

        public CreateProductCommandHandler(IProductService productService)
        {
            _productService = productService;
        }


        public async Task<Product> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            if (request.ProductImage != null && request.ProductImage.Length > 0)
            {
                // Generate a unique file name using GUID
                var fileName = Guid.NewGuid() + Path.GetExtension(request.ProductImage.FileName);
                var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                // Check if the directory exists, if not, create it
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Combine the directory and file name to get the full file path
                var filePath = Path.Combine(directoryPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.ProductImage.CopyToAsync(stream);
                }

                // Set the image URL in the product
                request.ImageUrl = fileName;
            }

            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                ImageUrl = request.ImageUrl
            };

            return await _productService.CreateProductAsync(product);
        }
    }
}
