using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class OrderDetailDAO
    {
        private readonly MyDBContext _context;

        public OrderDetailDAO(MyDBContext context)
        {
            _context = context;
        }
        public async Task<List<OrderDetail>> GetOrderDetailsAsync() => await _context.OrderDetails
            .Include(od => od.Product)
            .Include(od => od.Order)
            .ToListAsync();
        public async Task<OrderDetail> GetOrderDetailByIdAsync(int productId, int orderId) => await _context.OrderDetails
            .Include(od => od.Product)
            .Include(od => od.Order)
            .FirstOrDefaultAsync(od => od.ProductID == productId && od.OrderID == orderId);
        public async Task AddOrderDetailAsync(OrderDetail orderDetail)
        {
            await _context.OrderDetails.AddAsync(orderDetail);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateOrderDetailAsync(OrderDetail orderDetail)
        {
            _context.OrderDetails.Update(orderDetail);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteOrderDetailAsync(int productId, int orderId)
        {
            var orderDetail = await GetOrderDetailByIdAsync(productId, orderId);
            if (orderDetail != null)
            {
                _context.OrderDetails.Remove(orderDetail);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId)
        {
            return await _context.OrderDetails
                .Where(od => od.OrderID == orderId)
                .ToListAsync();
        }
    }
}
