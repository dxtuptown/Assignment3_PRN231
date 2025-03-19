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
    public class OrderDAO
    {
        private readonly MyDBContext _context;
        private readonly IMapper _mapper;

        public OrderDAO(MyDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<OrderDTO> GetAll()
        {
            var categories = _context.Orders.Include(p => p.OrderDetails).ToList();
            return _mapper.Map<List<OrderDTO>>(categories);
        }

        public OrderDTO GetById(int orderId)
        {
            var Order = _context.Orders.Find(orderId);
            return _mapper.Map<OrderDTO>(Order);
        }

        public void Add(OrderDTO order)
        {
            var Order = _mapper.Map<Order>(order);
            _context.Orders.Add(Order);
            _context.SaveChanges();
        }

        public void Update(OrderDTO product)
        {
            var Product = _mapper.Map<Order>(product);
            _context.Orders.Update(Product);
            _context.SaveChanges();
        }

        public void Delete(int orderid)
        {
            var Order = _context.Orders.Find(orderid);
            if (Order != null)
            {
                _context.Orders.Remove(Order);
                _context.SaveChanges();
            }
        }
    }
}
