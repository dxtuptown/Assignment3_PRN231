using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class ProductDAO
    {
        private readonly MyDBContext _context;

        public ProductDAO(MyDBContext context)
        {
            _context = context;
        }
        public async Task<List<Product>> GetProductsAsync() => await _context.Products.ToListAsync();
        public async Task<Product> GetProductByIdAsync(int id) => await _context.Products.FirstOrDefaultAsync(c => c.ProductID == id);
        public async Task AddProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteProductAsync(int id)
        {
            var product = await GetProductByIdAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<Product>> GetProductsByCategoryAsync(int categoryID)
        {
            return await _context.Products
                .Where(o => o.CategoryID == categoryID)
                .ToListAsync();
        }
    }
}
