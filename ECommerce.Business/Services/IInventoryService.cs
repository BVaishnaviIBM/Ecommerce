using System;
using System.Collections.Generic;
using System.Text;

using ECommerce.Models.Entities;

namespace ECommerce.Business.Services
{
    public interface IInventoryService
    {
        Task<List<Inventory>> GetAllInventoryAsync();

        Task<Inventory?> GetInventoryByIdAsync(int id);

        Task AddInventoryAsync(Inventory inventory);

        Task UpdateInventoryAsync(Inventory inventory);
    }
}
