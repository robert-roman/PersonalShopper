using PersonalShopper.DAL.Models;
using PersonalShopper.DAL.Repositories.GenericRepository;
using PersonalShopper.DAL;
using Microsoft.EntityFrameworkCore;

namespace PersonalShopper.Repositories.ProductRepository
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context) { }
        
        public async Task<Product> GetProductByName(string name) =>
            await _context.Products.Where(product => product.ProductName.Equals(name)).FirstOrDefaultAsync();

        public async Task<List<Product>> GetProductsWithCategory(string name) =>
            await _context.Products.Where(product => product.ProductCategory.Equals(name)).ToListAsync();

        public async Task<List<Product>> GetProductsContainingDescription(string name) =>
            await _context.Products.Where(product => product.ProductDescription.Contains(name)).ToListAsync();
    }
}
