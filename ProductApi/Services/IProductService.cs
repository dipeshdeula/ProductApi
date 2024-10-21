﻿using ProductApi.Models;

namespace ProductApi.Services
{
    public interface IProductService
    {
        Task <IEnumerable<Product>> GetAllProductsAsync();
        Task <Product> GetProductByIdAsync(int id);
        Task <Product>CreateProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int id);
        
        Task UploadFileAsync(ImageDetail imageDetail);

    }
}