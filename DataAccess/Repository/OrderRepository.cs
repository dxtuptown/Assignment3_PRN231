using AutoMapper;
using BusinessObject;
using DataAccess.DAO;
using DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class OrderRepository : Repository<OrderDTO>, IOrderRepository
    {
        public OrderRepository(MyDBContext context, OrderDAO dao)
            : base(context, dao) { }
    }
}
