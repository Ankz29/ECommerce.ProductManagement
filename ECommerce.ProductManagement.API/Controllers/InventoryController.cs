#region using directives

using ECommerce.ProductManagement.API.DTOs;
using ECommerce.ProductManagement.Core.Models;
using ECommerce.ProductManagement.Core.Services;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace ECommerce.ProductManagement.API.Controllers
{
    /// <summary>
    /// Inventory API Controller.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly InventoryService _inventoryService;

        public InventoryController(InventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        /// <summary>
        /// Retrieves all inventory records from the system.
        /// </summary>
        /// <returns>
        /// Returns 200 OK with the list of inventory items.
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> GetAllInventories()
        {
            var inventory = await _inventoryService.GetAllInventoryAsync();
            return Ok(inventory);
        }

        /// <summary>
        /// Retrieves inventory details for a specific product.
        /// </summary>
        /// <param name="productId">The product ID to look up.</param>
        /// <returns>
        /// Returns 200 OK with the inventory record if found, 
        /// or 404 Not Found if no inventory exists for the given product.
        /// </returns>
        [HttpGet("{productId}")]
        public async Task<IActionResult> GetInventoryByProductId(int productId)
        {
            var item = await _inventoryService.GetInventoryByProductIdAsync(productId);
            if (item == null)
                return NotFound();

            return Ok(item);
        }

        /// <summary>
        /// Creates a new inventory record.
        /// </summary>
        /// <param name="dto">The inventory details to create, including product ID and quantity.</param>
        /// <returns>
        /// Returns 201 Created with the newly created inventory record.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> CreateInventory(InventoryCreateDto dto)
        {
            var inventory = new Inventory
            {
                ProductId = dto.ProductId,
                Quantity = dto.Quantity
            };

            await _inventoryService.CreateInventoryAsync(inventory);

            var readDto = new InventoryReadDto
            {
                Id = inventory.Id,
                ProductId = inventory.ProductId,
                Quantity = inventory.Quantity
            };

            return CreatedAtAction(nameof(GetInventoryByProductId), new { productId = inventory.ProductId }, readDto);
        }

        /// <summary>
        /// Updates the quantity of an existing inventory record.
        /// </summary>
        /// <param name="productId">The product ID whose inventory should be updated.</param>
        /// <param name="quantity">The new quantity value.</param>
        /// <returns>
        /// Returns 204 No Content if the update succeeds, 
        /// or 404 Not Found if no inventory record exists for the given product.
        /// </returns>
        [HttpPut("{productId}")]
        public async Task<IActionResult> UpdateInventoryQuantity(int productId, [FromBody] int quantity)
        {
            var updated = await _inventoryService.UpdateInventoryQuantityAsync(productId, quantity);
            if (!updated)
                return NotFound();

            return NoContent();
        }
    }
}
