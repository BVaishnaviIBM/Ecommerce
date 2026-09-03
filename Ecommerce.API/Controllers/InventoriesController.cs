using Microsoft.AspNetCore.Mvc;

using ECommerce.Business.Services;
using ECommerce.Models.Entities;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoriesController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;
        private readonly ILogger<InventoriesController> _logger;

        public InventoriesController(
            IInventoryService inventoryService,
            ILogger<InventoriesController> logger)
        {
            _inventoryService = inventoryService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<Inventory>>> GetAllInventory()
        {
            _logger.LogInformation("Fetching all inventory records.");

            var inventory = await _inventoryService.GetAllInventoryAsync();

            _logger.LogInformation(
                "Retrieved {Count} inventory records.",
                inventory.Count);

            return Ok(inventory);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Inventory>> GetInventoryById(int id)
        {
            _logger.LogInformation(
                "Fetching inventory with InventoryId: {InventoryId}",
                id);

            var inventory =
                await _inventoryService.GetInventoryByIdAsync(id);

            if (inventory == null)
            {
                _logger.LogWarning(
                    "Inventory not found with InventoryId: {InventoryId}",
                    id);

                return NotFound();
            }

            return Ok(inventory);
        }

        [HttpPost]
        public async Task<ActionResult<Inventory>> AddInventory(
            Inventory inventory)
        {
            _logger.LogInformation(
                "Adding inventory for ProductId: {ProductId}",
                inventory.ProductId);

            await _inventoryService.AddInventoryAsync(inventory);

            _logger.LogInformation(
                "Inventory added successfully with InventoryId: {InventoryId}",
                inventory.InventoryId);

            return Ok(inventory);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Inventory>> UpdateInventory(
            int id,
            Inventory inventory)
        {
            if (id != inventory.InventoryId)
            {
                _logger.LogWarning(
                    "Inventory update failed. Route ID {RouteId} does not match InventoryId {InventoryId}.",
                    id,
                    inventory.InventoryId);

                return BadRequest();
            }

            _logger.LogInformation(
                "Updating inventory with InventoryId: {InventoryId}",
                id);

            await _inventoryService.UpdateInventoryAsync(inventory);

            _logger.LogInformation(
                "Inventory updated successfully with InventoryId: {InventoryId}",
                id);

            return Ok(inventory);
        }
    }
}