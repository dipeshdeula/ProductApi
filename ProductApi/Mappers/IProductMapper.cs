using ProductApi.Models;
using ProductApiAsync.DTOs;

namespace ProductApiAsync.Mappers
{
    public interface IProductMapper
    {
        ProductDto MapToDto(Product product);
        Product MapToEntity(ProductDto productDto);

    }
}
