using AutoMapper;
using BusinessObject;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly CategoryDAO _categoryDAO;
        private readonly IMapper _mapper;

        public CategoryRepository(CategoryDAO categoryDAO, IMapper mapper)
        {
            _categoryDAO = categoryDAO;
            _mapper = mapper;
        }

        public async Task AddCategoryAsync(Category category)
        {
            var entity = _mapper.Map<Category>(category);
            await _categoryDAO.AddCategoryAsync(entity);
        }

        public Task DeleteCategoryAsync(int id)
        {
            return _categoryDAO.DeleteCategoryAsync(id);
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return _mapper.Map<Category>(await _categoryDAO.GetCategoryByIdAsync(id));
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            return _mapper.Map<List<Category>>(await _categoryDAO.GetCategoriesAsync());
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            var entity = _mapper.Map<Category>(category);
            await _categoryDAO.UpdateCategoryAsync(entity);
        }
    }
}
