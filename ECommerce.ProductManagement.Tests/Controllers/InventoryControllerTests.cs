#region using directives

using ECommerce.ProductManagement.API.Controllers;
using ECommerce.ProductManagement.API.DTOs;
using ECommerce.ProductManagement.Core.Models;
using ECommerce.ProductManagement.Core.Services;
using ECommerce.ProductManagement.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;

#endregion

namespace ECommerce.ProductManagement.Tests.Controllers
{
    /// <summary>
    /// Inventory Controller Test class.
    /// </summary>
    public class InventoryControllerTests
    {
        /// <summary>
        /// Tests that GetAllInventories returns an OkObjectResult containing the expected list of inventory items.
        /// </summary>
        [Fact]
        public async Task GetAllInventories_ReturnsOk_WithInventoryList()
        {
            var items = new List<Inventory>
            {
                new Inventory { Id = 1, ProductId = 10, Quantity = 5 },
                new Inventory { Id = 2, ProductId = 11, Quantity = 3 }
            };

            var agent = new Mock<IInventoryServiceAgent>();
            agent.Setup(x => x.GetAllInventoryAsync()).ReturnsAsync(items);

            var service = new InventoryService(agent.Object);
            var controller = new InventoryController(service);

            var result = await controller.GetAllInventories();

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Same(items, ok.Value);
        }

        /// <summary>
        /// Tests that GetInventoryByProductId returns a NotFoundResult when the requested inventory item does not exist.
        /// </summary>
        [Fact]
        public async Task GetInventoryByProductId_ReturnsNotFound_WhenNull()
        {
            var agent = new Mock<IInventoryServiceAgent>();
            agent.Setup(x => x.GetInventoryByProductIdAsync(It.IsAny<int>())).ReturnsAsync((Inventory)null);

            var service = new InventoryService(agent.Object);
            var controller = new InventoryController(service);

            var result = await controller.GetInventoryByProductId(42);

            Assert.IsType<NotFoundResult>(result);
        }

        /// <summary>
        /// Tests that GetInventoryByProductId returns an OkObjectResult containing the expected inventory item when found.
        /// </summary>
        [Fact]
        public async Task GetInventoryByProductId_ReturnsOk_WithItem()
        {
            var item = new Inventory { Id = 5, ProductId = 20, Quantity = 7 };

            var agent = new Mock<IInventoryServiceAgent>();
            agent.Setup(x => x.GetInventoryByProductIdAsync(item.ProductId)).ReturnsAsync(item);

            var service = new InventoryService(agent.Object);
            var controller = new InventoryController(service);

            var result = await controller.GetInventoryByProductId(item.ProductId);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Same(item, ok.Value);
        }

        /// <summary>
        /// Tests that CreateInventory returns a CreatedAtActionResult with the correct route values,
        /// action name, and an InventoryReadDto containing the newly assigned Id, ProductId, and Quantity.
        /// </summary>
        [Fact]
        public async Task CreateInventory_ReturnsCreated_WithLocationAndBody()
        {
            var create = new InventoryCreateDto { ProductId = 77, Quantity = 9 };

            var agent = new Mock<IInventoryServiceAgent>();
            agent.Setup(x => x.AddInventoryAsync(It.IsAny<Inventory>())).Returns<Inventory>(inv =>
            {
                inv.Id = 444;
                return Task.CompletedTask;
            });

            var service = new InventoryService(agent.Object);
            var controller = new InventoryController(service);

            var result = await controller.CreateInventory(create);

            var created = Assert.IsType<CreatedAtActionResult>(result);
            var dto = Assert.IsType<InventoryReadDto>(created.Value);

            Assert.Equal(nameof(InventoryController.GetInventoryByProductId), created.ActionName);
            Assert.Equal(444, dto.Id);
            Assert.Equal(77, dto.ProductId);
            Assert.Equal(9, dto.Quantity);
            Assert.Equal(77, created.RouteValues!["productId"]);
        }

        [Fact]
        public async Task CreateInventory_AddsQuantityToExistingRecord()
        {
            var existing = new Inventory { Id = 12, ProductId = 77, Quantity = 2 };
            var create = new InventoryCreateDto { ProductId = 77, Quantity = 15 };
            var agent = new Mock<IInventoryServiceAgent>();
            agent.Setup(x => x.GetInventoryByProductIdAsync(77)).ReturnsAsync(existing);
            agent.Setup(x => x.UpdateInventoryAsync(It.Is<Inventory>(item => item.Id == 12 && item.Quantity == 17)))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var controller = new InventoryController(new InventoryService(agent.Object));

            var result = await controller.CreateInventory(create);

            var ok = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<InventoryReadDto>(ok.Value);
            Assert.Equal(12, dto.Id);
            Assert.Equal(77, dto.ProductId);
            Assert.Equal(17, dto.Quantity);
            agent.Verify(x => x.AddInventoryAsync(It.IsAny<Inventory>()), Times.Never);
            agent.Verify();
        }

        /// <summary>
        /// Tests that UpdateInventoryQuantity returns a NotFoundResult when the update fails because the inventory item does not exist.
        /// </summary>
        [Fact]
        public async Task UpdateInventoryQuantity_ReturnsNotFound_WhenUpdateFails()
        {
            var agent = new Mock<IInventoryServiceAgent>();
            // service will return null for GetInventoryByProductIdAsync causing update to return false
            agent.Setup(x => x.GetInventoryByProductIdAsync(It.IsAny<int>())).ReturnsAsync((Inventory)null);

            var service = new InventoryService(agent.Object);
            var controller = new InventoryController(service);

            var result = await controller.UpdateInventoryQuantity(5, 10);

            Assert.IsType<NotFoundResult>(result);
        }

        /// <summary>
        /// Tests that UpdateInventoryQuantity returns a NoContentResult when the update succeeds,
        /// and verifies that the service agent's UpdateInventoryAsync method is called once with the updated inventory.
        /// </summary>
        [Fact]
        public async Task UpdateInventoryQuantity_ReturnsNoContent_WhenSuccess()
        {
            var inventory = new Inventory { Id = 2, ProductId = 8, Quantity = 1 };

            var agent = new Mock<IInventoryServiceAgent>();
            agent.Setup(x => x.GetInventoryByProductIdAsync(inventory.ProductId)).ReturnsAsync(inventory);
            agent.Setup(x => x.UpdateInventoryAsync(It.IsAny<Inventory>())).Returns(Task.CompletedTask).Verifiable();

            var service = new InventoryService(agent.Object);
            var controller = new InventoryController(service);

            var result = await controller.UpdateInventoryQuantity(inventory.ProductId, 99);

            Assert.IsType<NoContentResult>(result);
            agent.Verify(x => x.UpdateInventoryAsync(It.Is<Inventory>(i => i.ProductId == inventory.ProductId && i.Quantity == 99)), Times.Once);
        }
    }
}
