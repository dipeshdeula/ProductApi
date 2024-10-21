using Microsoft.EntityFrameworkCore;
using ProductApi.Models;
using System.Collections.Generic;

namespace ProductApi.Data
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }

        public DbSet<ImageDetail> ImageDetails { get; set; }

        public DbSet<User> Users { get; set; }
    }
}
