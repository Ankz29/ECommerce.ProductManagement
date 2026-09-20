using ECommerce.ProductManagement.Core.Models;

namespace ECommerce.ProductManagement.Infrastructure.Repositories
{
    /// <summary>
    /// Interface class for Inventory.
    /// </summary>
    public interface IInventoryServiceAgent
    {
        /// <summary>
        /// Asynchronously retrieves all inventory from the data source.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation, containing a collection of inventory entities.
        /// </returns>
        Task<IEnumerable<Inventory>> GetAllInventoryAsync();

        /// <summary>
        /// Asynchronously retrieves a inventory by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the inventory.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing the inventory entity if found; otherwise null.
        /// </returns>
        Task<Inventory> GetInventoryByIdAsync(int id);

        /// <summary>
        /// Asynchronously adds a new inventory to the data source.
        /// </summary>
        /// <param name="inventory">The inventory entity to add.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        Task AddInventoryAsync(Inventory inventory);

        /// <summary>
        /// Asynchronously updates an existing inventory in the data source.
        /// </summary>
        /// <param name="inventory">The inventory entity with updated values.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        Task UpdateInventoryAsync(Inventory inventory);

        /// <summary>
        /// Asynchronously deletes a inventory from the data source by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the inventory to delete.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        Task DeleteInventoryAsync(int id);
    }
}