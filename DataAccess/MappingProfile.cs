using AutoMapper;
using BusinessObject;
using DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, Category>().ReverseMap();
            CreateMap<Order, Order>().ReverseMap();
            CreateMap<AuthDTO, AppUser>().ReverseMap();
            CreateMap<Product, Product>().ReverseMap();
        }
    }
}
