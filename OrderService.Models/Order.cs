using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        public int UserId { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        public string Status { get; set; } = "Pending";

        

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        
    }
}
