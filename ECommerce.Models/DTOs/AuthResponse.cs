using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Models.DTOs
{
    public class AuthResponse
    {
        public int UserId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;
    }
}
