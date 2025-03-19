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
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDAO _orderDAO;
        private readonly IMapper _mapper;

        public OrderRepository(OrderDAO orderDAO, IMapper mapper)
        {
            _orderDAO = orderDAO;
            _mapper = mapper;
        }

        public async Task AddOrderAsync(Order order)
        {
            var entity = _mapper.Map<Order>(order);
            await _orderDAO.AddOrderAsync(entity);
        }

        public Task DeleteOrderAsync(int id)
        {
            return _orderDAO.DeleteOrderAsync(id);
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return _mapper.Map<Order>(await _orderDAO.GetOrderByIdAsync(id));
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            return _mapper.Map<List<Order>>(await _orderDAO.GetOrdersAsync());
        }

        public async Task UpdateOrderAsync(Order order)
        {
            var entity = _mapper.Map<Order>(order);
            await _orderDAO.UpdateOrderAsync(entity);
        }
    }
}
