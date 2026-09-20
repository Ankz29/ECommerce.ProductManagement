#region using directives

using ECommerce.ProductManagement.Core.Models;
using ECommerce.ProductManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

#endregion

namespace ECommerce.ProductManagement.Infrastructure.Repositories
{
    /// <summary>
    /// Inventory Implementation service agent class.
    /// </summary>
    public class InventoryServiceAgent : IInventoryServiceAgent
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of InventoryServiceAgent with the specified database context.
        /// </summary>
        /// <param name="context">The ApplicationDbContext used for database operations.</param>
        public InventoryServiceAgent(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Asynchronously retrieves all inventory, including their associated Category and Inventory details.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation, containing a collection of Inventory entities.
        /// </returns>
        public async Task<IEnumerable<Inventory>> GetAllInventoryAsync() =>
            await _context.Inventories.Include(p => p.Product).Include(p => p.Id).ToListAsync();

        /// <summary>
        /// Asynchronously retrieves a inventory by its unique identifier, including Category and Inventory details.
        /// </summary>
        /// <param name="id">The unique identifier of the inventory.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing the Inventory entity if found; otherwise null.
        /// </returns>
        public async Task<Inventory> GetInventoryByIdAsync(int id) =>
            await _context.Inventories.Include(p => p.Product).Include(p => p.Id)
                                   .FirstOrDefaultAsync(p => p.Id == id);

        /// <summary>
        /// Asynchronously adds a new inventory to the database.
        /// </summary>
        /// <param name="inventory">The Inventory entity to add.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        public async Task AddInventoryAsync(Inventory inventory)
        {
            _context.Inventories.Add(inventory);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously updates an existing inventory in the database.
        /// </summary>
        /// <param name="inventory">The Inventory entity with updated values.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        public async Task UpdateInventoryAsync(Inventory inventory)
        {
            _context.Inventories.Update(inventory);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously deletes a inventory from the database by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the inventory to delete.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        public async Task DeleteInventoryAsync(int id)
        {
            var inventory = await _context.Inventories.FindAsync(id);
            if (inventory != null)
            {
                _context.Inventories.Remove(inventory);
                await _context.SaveChangesAsync();
            }
        }
    }
}