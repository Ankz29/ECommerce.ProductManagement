#region using directives

using ECommerce.ProductManagement.Core.Models;
using ECommerce.ProductManagement.Core.Services;
using ECommerce.ProductManagement.Infrastructure.Repositories;
using Moq;

#endregion

namespace ECommerce.ProductManagement.Tests.Services
{
    /// <summary>
    /// Product Service Test class.
    /// </summary>
    public class ProductServiceTests
    {
        /// <summary>
        /// Tests that AddProductAsync calls both the product and inventory service agents.
        /// Verifies that the product receives an assigned Id and that an inventory record
        /// is created with the correct ProductId.
        /// </summary>
        [Fact]
        public async Task AddProductAsync_CreatesInventoryWithInitialQuantity()
        {
            var product = new Product { Name = "Gadget" };

            var productAgent = new Mock<IProductServiceAgent>();
            var inventoryAgent = new Mock<IInventoryServiceAgent>();

            // Simulate repository assigning an Id when adding the product
            productAgent.Setup(x => x.AddProductAsync(It.IsAny<Product>())).Returns<Product>(p =>
            {
                p.Id = 321;
                return Task.CompletedTask;
            }).Verifiable();

            inventoryAgent.Setup(x => x.AddInventoryAsync(It.IsAny<Inventory>())).Returns(Task.CompletedTask).Verifiable();

            var service = new ProductService(productAgent.Object, inventoryAgent.Object);

            await service.AddProductAsync(product, 12);

            productAgent.Verify(x => x.AddProductAsync(product), Times.Once);
            inventoryAgent.Verify(x => x.AddInventoryAsync(It.Is<Inventory>(i => i.ProductId == 321 && i.Quantity == 12)), Times.Once);
        }

        /// <summary>
        /// Tests that GetAllProductsAsync returns the expected list of products
        /// provided by the mocked product service agent.
        /// </summary>
        [Fact]
        public async Task GetAllProductsAsync_ReturnsProducts()
        {
            var list = new List<Product>
            {
                new Product { Id = 1, Name = "A" },
                new Product { Id = 2, Name = "B" }
            };

            var productAgent = new Mock<IProductServiceAgent>();
            var inventoryAgent = new Mock<IInventoryServiceAgent>();

            productAgent.Setup(x => x.GetAllProductsAsync()).ReturnsAsync(list);

            var service = new ProductService(productAgent.Object, inventoryAgent.Object);

            var result = await service.GetAllProductsAsync();

            Assert.Same(list, result);
        }

        /// <summary>
        /// Tests that GetProductByIdAsync returns the expected product when it exists
        /// in the product service agent.
        /// </summary>
        [Fact]
        public async Task GetProductByIdAsync_ReturnsProduct_WhenExists()
        {
            var product = new Product { Id = 77, Name = "Widget" };

            var productAgent = new Mock<IProductServiceAgent>();
            var inventoryAgent = new Mock<IInventoryServiceAgent>();

            productAgent.Setup(x => x.GetProductByIdAsync(product.Id)).ReturnsAsync(product);

            var service = new ProductService(productAgent.Object, inventoryAgent.Object);

            var result = await service.GetProductByIdAsync(product.Id);

            Assert.Same(product, result);
        }

        /// <summary>
        /// Tests that UpdateProductAsync calls the product service agent's UpdateProductAsync method
        /// exactly once with the specified product.
        /// </summary>
        [Fact]
        public async Task UpdateProductAsync_CallsAgent()
        {
            var product = new Product { Id = 5, Name = "Updated" };

            var productAgent = new Mock<IProductServiceAgent>();
            var inventoryAgent = new Mock<IInventoryServiceAgent>();

            productAgent.Setup(x => x.UpdateProductAsync(It.IsAny<Product>())).Returns(Task.CompletedTask).Verifiable();

            var service = new ProductService(productAgent.Object, inventoryAgent.Object);

            await service.UpdateProductAsync(product);

            productAgent.Verify(x => x.UpdateProductAsync(product), Times.Once);
        }

        /// <summary>
        /// Tests that DeleteProductAsync calls the product service agent's DeleteProductAsync method
        /// exactly once with the specified product Id.
        /// </summary>
        [Fact]
        public async Task DeleteProductAsync_CallsAgent()
        {
            var productAgent = new Mock<IProductServiceAgent>();
            var inventoryAgent = new Mock<IInventoryServiceAgent>();

            productAgent.Setup(x => x.DeleteProductAsync(It.IsAny<int>())).Returns(Task.CompletedTask).Verifiable();

            var service = new ProductService(productAgent.Object, inventoryAgent.Object);

            await service.DeleteProductAsync(88);

            productAgent.Verify(x => x.DeleteProductAsync(88), Times.Once);
        }
    }
}
