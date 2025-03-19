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
    public class CategoryRepository : Repository<CategoryDTO>, ICategoryRepository
    {
        public CategoryRepository(MyDBContext context, CategoryDAO dao)
            : base(context, dao) { }
    }
}
