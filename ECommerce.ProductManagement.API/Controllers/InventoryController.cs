using ECommerce.ProductManagement.API.DTOs;
using ECommerce.ProductManagement.Core.Models;
using ECommerce.ProductManagement.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.ProductManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly InventoryService _inventoryService;

        public InventoryController(InventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        // ✅ GET /api/inventory
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var inventory = await _inventoryService.GetAllInventoryAsync();
            return Ok(inventory);
        }

        // ✅ GET /api/inventory/{productId}
        [HttpGet("{productId}")]
        public async Task<IActionResult> GetByProductId(int productId)
        {
            var item = await _inventoryService.GetInventoryByProductIdAsync(productId);
            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create(InventoryCreateDto dto)
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

            return CreatedAtAction(nameof(GetByProductId), new { productId = inventory.ProductId }, readDto);
        }

        // ✅ PUT /api/inventory/{productId}
        [HttpPut("{productId}")]
        public async Task<IActionResult> UpdateQuantity(int productId, [FromBody] int quantity)
        {
            var updated = await _inventoryService.UpdateInventoryQuantityAsync(productId, quantity);
            if (!updated)
                return NotFound();

            return NoContent();
        }
    }
}
