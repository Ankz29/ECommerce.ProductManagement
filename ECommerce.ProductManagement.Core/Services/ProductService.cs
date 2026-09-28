#region using directives

using ECommerce.ProductManagement.Core.Models;
using ECommerce.ProductManagement.Infrastructure.Repositories;

#endregion

namespace ECommerce.ProductManagement.Core.Services
{
    /// <summary>
    /// Product Service class.
    /// </summary>
    public class ProductService
    {
        private readonly IProductServiceAgent _productServiceAgent;
        private readonly IInventoryServiceAgent _inventoryServiceAgent;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductService"/> class.
        /// </summary>
        /// <param name="productAgent">
        /// The service agent responsible for interacting with the data source for products.
        /// </param>
        /// <param name="inventoryAgent">
        /// The service agent responsible for interacting with the data source for inventory.
        /// </param>
        public ProductService(IProductServiceAgent productAgent, IInventoryServiceAgent inventoryAgent)
        {
            _productServiceAgent = productAgent;
            _inventoryServiceAgent = inventoryAgent;
        }

        /// <summary>
        /// Adds a new product to the data source and automatically creates an associated inventory record.
        /// </summary>
        /// <param name="product">The <see cref="Product"/> object to add.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        public async Task AddProductAsync(Product product, int initialQuantity = 0)
        {
            await _productServiceAgent.AddProductAsync(product);

            var inventory = new Inventory
            {
                ProductId = product.Id,
                Quantity = initialQuantity,
            };

            await _inventoryServiceAgent.AddInventoryAsync(inventory);
        }

        /// <summary>
        /// Retrieves all products from the data source.
        /// </summary>
        /// <returns>
        /// A collection of <see cref="Product"/> objects representing all products.
        /// </returns>
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _productServiceAgent.GetAllProductsAsync();
        }

        /// <summary>
        /// Retrieves a product by its unique identifier.
        /// </summary>
        /// <param name="id">The product ID to look up.</param>
        /// <returns>
        /// The <see cref="Product"/> object if found, or null if no product exists with the given ID.
        /// </returns>
        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _productServiceAgent.GetProductByIdAsync(id);
        }

        /// <summary>
        /// Updates an existing product in the data source.
        /// </summary>
        /// <param name="product">The <see cref="Product"/> object containing updated details.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        public async Task UpdateProductAsync(Product product)
        {
            await _productServiceAgent.UpdateProductAsync(product);
        }

        /// <summary>
        /// Deletes a product from the data source by its unique identifier.
        /// </summary>
        /// <param name="id">The product ID to delete.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        public async Task DeleteProductAsync(int id)
        {
            await _productServiceAgent.DeleteProductAsync(id);
        }
    }
}