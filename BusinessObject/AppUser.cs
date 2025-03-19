using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class AppUser : IdentityUser
    {
        public ICollection<Order>? Orders { get; set; }
    }
}
