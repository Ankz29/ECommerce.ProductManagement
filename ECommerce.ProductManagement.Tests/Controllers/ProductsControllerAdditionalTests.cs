using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerce.ProductManagement.API.Controllers;
using ECommerce.ProductManagement.API.DTOs;
using ECommerce.ProductManagement.Core.Models;
using ECommerce.ProductManagement.Core.Services;
using ECommerce.ProductManagement.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ECommerce.ProductManagement.Tests.Controllers
{
    public class ProductsControllerAdditionalTests
    {
        [Fact]
        public async Task GetAllProducts_ReturnsOk_WithEmptyList()
        {
            var productAgent = new Mock<IProductServiceAgent>();
            var inventoryAgent = new Mock<IInventoryServiceAgent>();
            var empty = new List<Product>();
            productAgent.Setup(x => x.GetAllProductsAsync()).ReturnsAsync(empty);

            var productService = new ProductService(productAgent.Object, inventoryAgent.Object);
            var controller = new ProductsController(productService);

            var result = await controller.GetAllProducts();

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Same(empty, ok.Value);
        }

        [Fact]
        public async Task CreateProduct_CallsInventoryAdd_WithProductIdSetByRepository()
        {
            var create = new ProductCreateDto { Name = "Gadget", Description = "Desc", Price = 5m, CategoryId = 2, InventoryId = 0 };

            var productAgent = new Mock<IProductServiceAgent>();
            var inventoryAgent = new Mock<IInventoryServiceAgent>();

            // Simulate repository assigning Id
            productAgent.Setup(x => x.AddProductAsync(It.IsAny<Product>())).Returns<Product>(p =>
            {
                p.Id = 77;
                return Task.CompletedTask;
            });

            // Verify inventory created with ProductId equal to assigned product Id
            inventoryAgent.Setup(x => x.AddInventoryAsync(It.Is<Core.Models.Inventory>(inv => inv.ProductId == 77)))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var productService = new ProductService(productAgent.Object, inventoryAgent.Object);
            var controller = new ProductsController(productService);

            var result = await controller.CreateProduct(create);

            var created = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(77, created.RouteValues!["id"]);
            inventoryAgent.Verify();
        }
    }
}
