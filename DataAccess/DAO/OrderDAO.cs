using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class OrderDAO
    {
        private readonly MyDBContext _context;

        public OrderDAO(MyDBContext context)
        {
            _context = context;
        }
        public async Task<List<Order>> GetOrdersAsync() => await _context.Orders.ToListAsync();
        public async Task<Order> GetOrderByIdAsync(int id) => await _context.Orders.FirstOrDefaultAsync(c => c.OrderID == id);
        public async Task AddOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteOrderAsync(int id)
        {
            var order = await GetOrderByIdAsync(id);    
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }
    }
}
