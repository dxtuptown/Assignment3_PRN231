using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace BusinessObject
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }
        public int MemberID { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public DateTime RequiredDate { get; set; } = DateTime.Now;
        public DateTime ShippedDate { get; set; } = DateTime.Now;
        public decimal Freight { get; set; }
        public AppUser? User { get; set; }
        [JsonIgnore]
        public ICollection<OrderDetail>? OrderDetails { get; set; }

    }
}
