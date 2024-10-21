using Microsoft.EntityFrameworkCore;
using ProductApi.Data;
using ProductApi.Models;
using System.Collections.Generic;
using ProductApi.Services;
using ProductApi.Repositories;

namespace ProductApi
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }


        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllProductsAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {

            return await _productRepository.GetProductByIdAsync(id);
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
           await _productRepository.AddProductAsync(product);
            return product;
        }
        public async Task  UpdateProductAsync(Product product)
        {
            await _productRepository.UpdateProductAsync(product);
        }
        public async Task DeleteProductAsync(int id)
        {
            await _productRepository.DeleteProductAsync(id);
        }

        public async Task UploadFileAsync(ImageDetail imageDetail)
        {
            await _productRepository.UploadFileAsync(imageDetail);
        }
    }
}
