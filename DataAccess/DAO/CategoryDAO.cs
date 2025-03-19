using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class CategoryDAO
    {
        private readonly MyDBContext _context;

        public CategoryDAO(MyDBContext context)
        {
            _context = context;
        }
        public async Task<List<Category>> GetCategoriesAsync() => await _context.Categories.ToListAsync();
        public async Task<Category> GetCategoryByIdAsync(int id) => await _context.Categories.FirstOrDefaultAsync(c => c.CategoryID == id);
        public async Task AddCategoryAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateCategoryAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteCategoryAsync(int id)
        {
            var category = await GetCategoryByIdAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
    }
}
