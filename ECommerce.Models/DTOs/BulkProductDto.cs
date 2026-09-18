using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Models.DTOs
{
    public class BulkProductDto
    {
        public string ProductName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
    }
}
