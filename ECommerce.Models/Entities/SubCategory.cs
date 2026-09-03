using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.Entities
{
    public class SubCategory
    {
        public int SubCategoryId { get; set; }

        public string SubCategoryName { get; set; } = string.Empty;

        public int CategoryId { get; set; }

        public Category? Category { get; set; }
    }
}
