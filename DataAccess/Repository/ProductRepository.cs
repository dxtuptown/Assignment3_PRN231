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
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDAO _productDAO;
        private readonly IMapper _mapper;

        public ProductRepository(ProductDAO productDAO, IMapper mapper)
        {
            _productDAO = productDAO;
            _mapper = mapper;
        }

        public async Task AddProductAsync(Product product)
        {
            var entity = _mapper.Map<Product>(product);
            await _productDAO.AddProductAsync(entity);
        }

        public Task DeleteProductAsync(int id)
        {
            return _productDAO.DeleteProductAsync(id);
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return _mapper.Map<Product>(await _productDAO.GetProductByIdAsync(id));
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            return _mapper.Map<List<Product>>(await _productDAO.GetProductsAsync());
        }

        public async Task<List<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            var products = await _productDAO.GetProductsByCategoryAsync(categoryId);
            return _mapper.Map<List<Product>>(products);
        }

        public async Task UpdateProductAsync(Product product)
        {
            var entity = _mapper.Map<Product>(product);
            await _productDAO.UpdateProductAsync(entity);
        }
    }
}
