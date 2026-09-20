#region using directives

using ECommerce.ProductManagement.Core.Models;
using ECommerce.ProductManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

#endregion

namespace ECommerce.ProductManagement.Infrastructure.Repositories
{
    /// <summary>
    /// Product Implementation service agent class.
    /// </summary>
    public class ProductServiceAgent : IProductServiceAgent
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the ProductServiceAgent with the specified database context.
        /// </summary>
        /// <param name="context">The ApplicationDbContext used for database operations.</param>
        public ProductServiceAgent(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Asynchronously retrieves all products, including their associated Category and Inventory details.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation, containing a collection of Product entities.
        /// </returns>
        public async Task<IEnumerable<Product>> GetAllProductsAsync() =>
            await _context.Products.Include(p => p.Category).Include(p => p.Inventory).ToListAsync();

        /// <summary>
        /// Asynchronously retrieves a product by its unique identifier, including Category and Inventory details.
        /// </summary>
        /// <param name="id">The unique identifier of the product.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing the Product entity if found; otherwise null.
        /// </returns>
        public async Task<Product> GetProductByIdAsync(int id) =>
            await _context.Products.Include(p => p.Category).Include(p => p.Inventory)
                                   .FirstOrDefaultAsync(p => p.Id == id);

        /// <summary>
        /// Asynchronously adds a new product to the database.
        /// </summary>
        /// <param name="product">The Product entity to add.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        public async Task AddProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously updates an existing product in the database.
        /// </summary>
        /// <param name="product">The Product entity with updated values.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        public async Task UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously deletes a product from the database by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the product to delete.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}