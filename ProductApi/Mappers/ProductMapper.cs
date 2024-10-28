using ProductApi.Models;
using ProductApiAsync.DTOs;

namespace ProductApiAsync.Mappers
{
    public class ProductMapper: IProductMapper
    {
        public ProductDto MapToDto(Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                //ImageUrl = product.ImageUrl
            };
        }

        public Product MapToEntity(ProductDto productDto)
        {
            return new Product
            {

                Id = productDto.Id,
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
               // ImageUrl = productDto.ImageUrl

            };
        }

    }
}
