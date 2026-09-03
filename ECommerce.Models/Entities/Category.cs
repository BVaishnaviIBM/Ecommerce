using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ECommerce.Models.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        [JsonIgnore]
        public ICollection<SubCategory> SubCategories { get; set; } = new List<SubCategory>();
    }
}
