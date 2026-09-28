#region using directives

using ECommerce.ProductManagement.API.DTOs;
using ECommerce.ProductManagement.Core.Models;
using ECommerce.ProductManagement.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace ECommerce.ProductManagement.API.Controllers
{
    /// <summary>
    /// Inventory API Controller.
    /// </summary>
    [ApiController]
    // Match Swagger route (/api/Inventory) so requests from clients and debugger hit this controller
    [Route("api/Inventory")]
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
        [Authorize(Roles = "Admin,User")]
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
        [Authorize(Roles = "Admin")]
        [HttpGet("{productId}")]
        public async Task<IActionResult> GetInventoryByProductId(int productId)
        {
            var item = await _inventoryService.GetInventoryByProductIdAsync(productId);
            if (item == null)
                return NotFound();

            return Ok(item);
        }

        /// <summary>
        /// Adds quantity to the product's inventory, creating a record if needed.
        /// </summary>
        /// <param name="dto">The inventory details to create, including product ID and quantity.</param>
        /// <returns>
        /// Returns 201 Created for a new record or 200 OK after adding to an existing record.
        /// </returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateInventory(InventoryCreateDto dto)
        {
            var existing = await _inventoryService.GetInventoryByProductIdAsync(dto.ProductId);
            if (existing != null)
            {
                var newQuantity = existing.Quantity + dto.Quantity;
                var updated = await _inventoryService.UpdateInventoryQuantityAsync(dto.ProductId, newQuantity);
                if (!updated)
                    return NotFound();

                return Ok(new InventoryReadDto
                {
                    Id = existing.Id,
                    ProductId = existing.ProductId,
                    Quantity = newQuantity
                });
            }

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
        [Authorize(Roles = "Admin")]
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
