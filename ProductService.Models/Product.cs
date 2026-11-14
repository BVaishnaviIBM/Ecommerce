using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ProductService.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int CategoryId { get; set; }

        public Category? Category { get; set; }

        [JsonIgnore]
        public Inventory? Inventory { get; set; }

        
    }
}
