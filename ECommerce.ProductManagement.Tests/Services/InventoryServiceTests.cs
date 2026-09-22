#region using directives

using ECommerce.ProductManagement.Core.Models;
using ECommerce.ProductManagement.Core.Services;
using ECommerce.ProductManagement.Infrastructure.Repositories;
using Moq;

#endregion

namespace ECommerce.ProductManagement.Tests.Services
{
    /// <summary>
    /// Inventory Service Test class.
    /// </summary>
    public class InventoryServiceTests
    {
        // <summary>
        /// Tests that GetAllInventoryAsync returns the expected list of inventory items
        /// provided by the mocked inventory service agent.
        /// </summary>
        [Fact]
        public async Task GetAllInventoryAsync_ReturnsInventoryList()
        {
            var list = new List<Inventory>
            {
                new Inventory { Id = 1, ProductId = 10, Quantity = 2 },
                new Inventory { Id = 2, ProductId = 11, Quantity = 4 }
            };

            var agent = new Mock<IInventoryServiceAgent>();
            agent.Setup(x => x.GetAllInventoryAsync()).ReturnsAsync(list);

            var service = new InventoryService(agent.Object);

            var result = await service.GetAllInventoryAsync();

            Assert.Same(list, result);
        }

        /// <summary>
        /// Tests that GetInventoryByProductIdAsync returns the expected inventory item
        /// when it exists in the inventory service agent.
        /// </summary>
        [Fact]
        public async Task GetInventoryByProductIdAsync_ReturnsInventory_WhenExists()
        {
            var inv = new Inventory { Id = 5, ProductId = 20, Quantity = 7 };

            var agent = new Mock<IInventoryServiceAgent>();
            agent.Setup(x => x.GetInventoryByIdAsync(inv.ProductId)).ReturnsAsync(inv);

            var service = new InventoryService(agent.Object);

            var result = await service.GetInventoryByProductIdAsync(inv.ProductId);

            Assert.Same(inv, result);
        }

        /// <summary>
        /// Tests that CreateInventoryAsync calls the inventory service agent's AddInventoryAsync method
        /// and returns the same inventory object that was passed in.
        /// </summary>
        [Fact]
        public async Task CreateInventoryAsync_CallsAddAndReturnsInventory()
        {
            var inv = new Inventory { ProductId = 33, Quantity = 8 };

            var agent = new Mock<IInventoryServiceAgent>();
            agent.Setup(x => x.AddInventoryAsync(It.IsAny<Inventory>())).Returns(Task.CompletedTask).Verifiable();

            var service = new InventoryService(agent.Object);

            var result = await service.CreateInventoryAsync(inv);

            Assert.Same(inv, result);
            agent.Verify(x => x.AddInventoryAsync(inv), Times.Once);
        }

        /// <summary>
        /// Tests that UpdateInventoryQuantityAsync returns false when the inventory item
        /// is not found, and verifies that UpdateInventoryAsync is never called.
        /// </summary>
        [Fact]
        public async Task UpdateInventoryQuantityAsync_ReturnsFalse_WhenInventoryNotFound()
        {
            var agent = new Mock<IInventoryServiceAgent>();
            agent.Setup(x => x.GetInventoryByIdAsync(It.IsAny<int>())).ReturnsAsync((Inventory)null);
            agent.Setup(x => x.UpdateInventoryAsync(It.IsAny<Inventory>())).Returns(Task.CompletedTask);

            var service = new InventoryService(agent.Object);

            var result = await service.UpdateInventoryQuantityAsync(99, 5);

            Assert.False(result);
            agent.Verify(x => x.UpdateInventoryAsync(It.IsAny<Inventory>()), Times.Never);
        }

        /// <summary>
        /// Tests that UpdateInventoryQuantityAsync returns true when the inventory item exists,
        /// updates its quantity, and verifies that UpdateInventoryAsync is called once with the updated inventory.
        /// </summary>
        [Fact]
        public async Task UpdateInventoryQuantityAsync_ReturnsTrue_AndUpdatesQuantity()
        {
            var inv = new Inventory { Id = 3, ProductId = 12, Quantity = 1 };

            var agent = new Mock<IInventoryServiceAgent>();
            agent.Setup(x => x.GetInventoryByIdAsync(inv.ProductId)).ReturnsAsync(inv);
            agent.Setup(x => x.UpdateInventoryAsync(It.IsAny<Inventory>())).Returns(Task.CompletedTask).Verifiable();

            var service = new InventoryService(agent.Object);

            var updated = await service.UpdateInventoryQuantityAsync(inv.ProductId, 42);

            Assert.True(updated);
            agent.Verify(x => x.UpdateInventoryAsync(It.Is<Inventory>(i => i.ProductId == inv.ProductId && i.Quantity == 42)), Times.Once);
        }
    }
}
