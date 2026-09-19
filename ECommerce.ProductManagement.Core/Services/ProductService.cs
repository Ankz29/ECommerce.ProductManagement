#region using directives

using ECommerce.ProductManagement.Core.Models;
using ECommerce.ProductManagement.Infrastructure.Repositories;

#endregion

namespace ECommerce.ProductManagement.Core.Services
{
    public class ProductService
    {
        private readonly IProductServiceAgent _productServiceAgent;
        private readonly IInventoryServiceAgent _inventoryServiceAgent;

        public ProductService(IProductServiceAgent productAgent, IInventoryServiceAgent inventoryAgent)
        {
            _productServiceAgent = productAgent;
            _inventoryServiceAgent = inventoryAgent;
        }

        public async Task AddProductAsync(Product product, int initialQuantity)
        {
            await _productServiceAgent.AddAsync(product);

            var inventory = new Inventory
            {
                ProductId = product.Id,
                Quantity = initialQuantity
            };

            await _inventoryServiceAgent.AddAsync(inventory);
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _productServiceAgent.GetAllAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _productServiceAgent.GetByIdAsync(id);
        }

        public async Task UpdateProductAsync(Product product)
        {
            await _productServiceAgent.UpdateAsync(product);
        }

        public async Task DeleteProductAsync(int id)
        {
            await _productServiceAgent.DeleteAsync(id);
        }
    }
}