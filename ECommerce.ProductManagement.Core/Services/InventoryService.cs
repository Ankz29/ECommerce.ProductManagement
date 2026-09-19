using ECommerce.ProductManagement.Core.Models;
using ECommerce.ProductManagement.Infrastructure.Repositories;

namespace ECommerce.ProductManagement.Core.Services
{
    public class InventoryService
    {
        private readonly IInventoryServiceAgent _inventoryServiceAgent;

        public InventoryService(IInventoryServiceAgent inventoryServiceAgent)
        {
            _inventoryServiceAgent = inventoryServiceAgent;
        }

        public async Task<IEnumerable<Inventory>> GetAllInventoryAsync() =>
            await _inventoryServiceAgent.GetAllAsync();

        public async Task<Inventory?> GetInventoryByProductIdAsync(int productId) =>
            await _inventoryServiceAgent.GetByIdAsync(productId);

        public async Task<Inventory> CreateInventoryAsync(Inventory inventory)
        {
            await _inventoryServiceAgent.AddAsync(inventory);
            return inventory;
        }

        public async Task<bool> UpdateInventoryQuantityAsync(int productId, int quantity)
        {
            var inventory = await _inventoryServiceAgent.GetByIdAsync(productId);
            if (inventory == null) return false;

            inventory.Quantity = quantity;
            await _inventoryServiceAgent.UpdateAsync(inventory);
            return true;
        }
    }
}
