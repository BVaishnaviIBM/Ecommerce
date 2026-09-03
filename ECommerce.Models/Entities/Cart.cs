using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Models.Entities
{
    public class Cart
    {
        public int CartId { get; set; }

        public int UserId { get; set; }

        public User User { get; set; } = null!;

        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
