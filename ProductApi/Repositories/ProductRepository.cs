using Microsoft.EntityFrameworkCore;
using ProductApi.Data;
using ProductApi.Models;

namespace ProductApi.Repositories
{
    public class ProductRepository : IProductRepository
    {
        //product list data manually
        // private readonly List<Product> _products;


        //for manual
        /* public ProductRepository()
         { 
             _products = new List<Product>
             {
                 new Product { Id = 1, Name = "Keyboard", Price = 20 },
                 new Product { Id = 2, Name = "Mouse", Price = 10 },
                 new Product { Id = 3, Name = "Monitor", Price = 200 }
             };
         }*/


        //Get product data list from the database
        private readonly ProductDbContext _context;

        //for database
        public ProductRepository(ProductDbContext context)
        {
            _context = context;
            
        }
        public async Task<IEnumerable<Product>>GetAllProductsAsync()
        {
            //return _products;
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            //return _products.FirstOrDefault(p => p.Id == id);
            //return await _context.Products.FindAsync(id) ?? throw new KeyNotFoundException("Product Not Found");
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                throw new KeyNotFoundException("Product Not Found");
            }
            return product;
        }

        public async Task AddProductAsync(Product product)
        {
          
           await _context.Products.AddAsync(product);
           await _context.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            
            var productDetail = await _context.Products.FindAsync(product.Id);

            if (productDetail == null)
            {
                //Handle the case where the product doesn't exist
                throw new KeyNotFoundException("Product Not found");
            }
            productDetail.Name = product.Name;
            productDetail.Description = product.Description;
            productDetail.Price = product.Price;

            await _context.SaveChangesAsync();

        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
               _context.Products.Remove(product);
               await _context.SaveChangesAsync();
                
            }
        }



        public async Task UploadFileAsync(ImageDetail imageDetails)
        {
            await _context.ImageDetails.AddAsync(imageDetails);
            await _context.SaveChangesAsync();
        }
    }
}
