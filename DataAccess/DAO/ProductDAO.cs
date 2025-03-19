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
    public class ProductDAO
    {
        private readonly MyDBContext _context;
        private readonly IMapper _mapper;

        public ProductDAO(MyDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<ProductDTO> GetAll()
        {
            var products = _context.Products.Include(p => p.Category).ToList();
            return _mapper.Map<List<ProductDTO>>(products);
        }

        public ProductDTO GetById(int ProductId)
        {
            var category = _context.Products.Find(ProductId);
            return _mapper.Map<ProductDTO>(category);
        }

        public void Add(ProductDTO product)
        {
            var Product = _mapper.Map<Product>(product);
            _context.Products.Add(Product);
            _context.SaveChanges();
        }

        public void Update(ProductDTO product)
        {
            var Product = _mapper.Map<Product>(product);
            _context.Products.Update(Product);
            _context.SaveChanges();
        }

        public void Delete(int productid)
        {
            var Product = _context.Products.Find(productid);
            if (Product != null)
            {
                _context.Products.Remove(Product);
                _context.SaveChanges();
            }
        }
    }
}
