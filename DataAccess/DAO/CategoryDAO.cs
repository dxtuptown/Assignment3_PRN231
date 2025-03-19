using AutoMapper;
using BusinessObject;
using DataAccess.DTO;
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
        private readonly IMapper _mapper;

        public CategoryDAO(MyDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<CategoryDTO> GetAll()
        {
            var categories = _context.Categories.ToList();
            return _mapper.Map<List<CategoryDTO>>(categories);
        }

        public CategoryDTO GetById(int categoryId)
        {
            var category = _context.Categories.Find(categoryId);
            return _mapper.Map<CategoryDTO>(category);
        }

        public void Add(CategoryDTO categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public void Update(CategoryDTO categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            _context.Categories.Update(category);
            _context.SaveChanges();
        }

        public void Delete(int categoryId)
        {
            var category = _context.Categories.Find(categoryId);
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
        }
    }
}
