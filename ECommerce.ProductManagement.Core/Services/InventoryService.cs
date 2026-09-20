using ECommerce.ProductManagement.Core.Models;
using ECommerce.ProductManagement.Infrastructure.Repositories;

namespace ECommerce.ProductManagement.Core.Services
{
    /// <summary>
    /// Inventory Service class.
    /// </summary>
    public class InventoryService
    {
        private readonly IInventoryServiceAgent _inventoryServiceAgent;

        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryService"/> class.
        /// </summary>
        /// <param name="inventoryServiceAgent">
        /// The service agent responsible for interacting with the data source for inventory.
        /// </param>
        public InventoryService(IInventoryServiceAgent inventoryServiceAgent)
        {
            _inventoryServiceAgent = inventoryServiceAgent;
        }

        /// <summary>
        /// Retrieves all inventory records from the data source.
        /// </summary>
        /// <returns>
        /// A collection of <see cref="Inventory"/> objects representing all inventory items.
        /// </returns>
        public async Task<IEnumerable<Inventory>> GetAllInventoryAsync() =>
            await _inventoryServiceAgent.GetAllInventoryAsync();

        /// <summary>
        /// Retrieves the inventory record for a specific product.
        /// </summary>
        /// <param name="productId">The product ID to look up.</param>
        /// <returns>
        /// The <see cref="Inventory"/> record if found, or null if no inventory exists for the given product.
        /// </returns>
        public async Task<Inventory?> GetInventoryByProductIdAsync(int productId) =>
            await _inventoryServiceAgent.GetInventoryByIdAsync(productId);

        /// <summary>
        /// Creates a new inventory record in the data source.
        /// </summary>
        /// <param name="inventory">The <see cref="Inventory"/> object to add.</param>
        /// <returns>
        /// The newly created <see cref="Inventory"/> object.
        /// </returns>
        public async Task<Inventory> CreateInventoryAsync(Inventory inventory)
        {
            await _inventoryServiceAgent.AddInventoryAsync(inventory);
            return inventory;
        }

        /// <summary>
        /// Updates the quantity of an existing inventory record.
        /// </summary>
        /// <param name="productId">The product ID whose inventory should be updated.</param>
        /// <param name="quantity">The new quantity value.</param>
        /// <returns>
        /// True if the update succeeds, or false if no inventory record exists for the given product.
        /// </returns>
        public async Task<bool> UpdateInventoryQuantityAsync(int productId, int quantity)
        {
            var inventory = await _inventoryServiceAgent.GetInventoryByIdAsync(productId);
            if (inventory == null) return false;

            inventory.Quantity = quantity;
            await _inventoryServiceAgent.UpdateInventoryAsync(inventory);
            return true;
        }
    }
}
