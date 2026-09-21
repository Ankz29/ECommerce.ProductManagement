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
    /// <summary>
    /// Products Controller Test class.
    /// </summary>
    public class ProductsControllerTests
    {
        /// <summary>
        /// Verifies that GetAllProducts returns 200 OK with the list of products when products exist.
        /// </summary>
        [Fact]
        public async Task GetAllProducts_ReturnsOk_WithProducts()
        {
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "A", Price = 1 },
                new Product { Id = 2, Name = "B", Price = 2 }
            };

            var productAgent = new Mock<IProductServiceAgent>();
            var inventoryAgent = new Mock<IInventoryServiceAgent>();
            productAgent.Setup(x => x.GetAllProductsAsync()).ReturnsAsync(products);

            var productService = new ProductService(productAgent.Object, inventoryAgent.Object);
            var controller = new ProductsController(productService);

            var result = await controller.GetAllProducts();

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Same(products, ok.Value);
        }

        /// <summary>
        /// Verifies that GetProductById returns 404 Not Found when the product does not exist.
        /// </summary>
        [Fact]
        public async Task GetProductById_ReturnsNotFound_WhenProductIsNull()
        {
            var productAgent = new Mock<IProductServiceAgent>();
            var inventoryAgent = new Mock<IInventoryServiceAgent>();
            productAgent.Setup(x => x.GetProductByIdAsync(It.IsAny<int>())).ReturnsAsync((Product)null);

            var productService = new ProductService(productAgent.Object, inventoryAgent.Object);
            var controller = new ProductsController(productService);

            var result = await controller.GetProductById(5);

            Assert.IsType<NotFoundResult>(result);
        }

        /// <summary>
        /// Verifies that GetProductById returns 200 OK with a ProductReadDto when the product exists.
        /// </summary>
        [Fact]
        public async Task GetProductById_ReturnsOk_WithDto()
        {
            var product = new Product { Id = 10, Name = "Widget", Price = 9.99m };

            var productAgent = new Mock<IProductServiceAgent>();
            var inventoryAgent = new Mock<IInventoryServiceAgent>();
            productAgent.Setup(x => x.GetProductByIdAsync(product.Id)).ReturnsAsync(product);

            var productService = new ProductService(productAgent.Object, inventoryAgent.Object);
            var controller = new ProductsController(productService);

            var result = await controller.GetProductById(product.Id);

            var ok = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<ProductReadDto>(ok.Value);
            Assert.Equal(product.Id, dto.Id);
            Assert.Equal(product.Name, dto.Name);
            Assert.Equal(product.Price, dto.Price);
        }

        /// <summary>
        /// Verifies that CreateProduct returns 201 Created with correct location and body when a product is successfully created.
        /// </summary>
        [Fact]
        public async Task CreateProduct_ReturnsCreated_WithLocationAndBody()
        {
            var create = new ProductCreateDto { Name = "Wireless Mouse", Description = "Description for Wireless Mouse", Price = 3.5m, CategoryId = 1, InventoryId = 2 };

            var productAgent = new Mock<IProductServiceAgent>();
            var inventoryAgent = new Mock<IInventoryServiceAgent>();

            // Simulate repository setting the Id when adding
            productAgent.Setup(x => x.AddProductAsync(It.IsAny<Product>())).Returns<Product>(p =>
            {
                p.Id = 123;
                return Task.CompletedTask;
            });
            inventoryAgent.Setup(x => x.AddInventoryAsync(It.IsAny<Core.Models.Inventory>())).Returns(Task.CompletedTask);

            var productService = new ProductService(productAgent.Object, inventoryAgent.Object);
            var controller = new ProductsController(productService);

            var result = await controller.CreateProduct(create);

            var created = Assert.IsType<CreatedAtActionResult>(result);
            // Controller returns an anonymous object with Id, Name and Price
            var value = created.Value!;

            Assert.Equal(nameof(ProductsController.GetProductById), created.ActionName);

            var idProp = value.GetType().GetProperty("Id");
            var nameProp = value.GetType().GetProperty("Name");
            var priceProp = value.GetType().GetProperty("Price");

            Assert.NotNull(idProp);
            Assert.NotNull(nameProp);
            Assert.NotNull(priceProp);

            Assert.Equal(123, (int)idProp.GetValue(value)!);
            Assert.Equal("Wireless Mouse", (string)nameProp.GetValue(value)!);
            Assert.Equal(3.5m, (decimal)priceProp.GetValue(value)!);
            Assert.Equal(123, created.RouteValues!["id"]);
        }

        /// <summary>
        /// Verifies that UpdateProduct returns 400 Bad Request when the provided ID does not match the product’s ID.
        /// </summary>
        [Fact]
        public async Task UpdateProduct_ReturnsBadRequest_WhenIdMismatch()
        {
            var productAgent = new Mock<IProductServiceAgent>();
            var inventoryAgent = new Mock<IInventoryServiceAgent>();
            var productService = new ProductService(productAgent.Object, inventoryAgent.Object);
            var controller = new ProductsController(productService);

            var product = new Product { Id = 2, Name = "X" };

            var result = await controller.UpdateProduct(1, product);

            Assert.IsType<BadRequestResult>(result);
        }

        /// <summary>
        /// Verifies that UpdateProduct returns 204 No Content and calls the service when the update succeeds.
        /// </summary>
        [Fact]
        public async Task UpdateProduct_ReturnsNoContent_WhenSuccess()
        {
            var productAgent = new Mock<IProductServiceAgent>();
            var inventoryAgent = new Mock<IInventoryServiceAgent>();
            productAgent.Setup(x => x.UpdateProductAsync(It.IsAny<Product>())).Returns(Task.CompletedTask).Verifiable();

            var productService = new ProductService(productAgent.Object, inventoryAgent.Object);
            var controller = new ProductsController(productService);

            var product = new Product { Id = 5, Name = "Updated" };

            var result = await controller.UpdateProduct(5, product);

            Assert.IsType<NoContentResult>(result);
            productAgent.Verify(x => x.UpdateProductAsync(product), Times.Once);
        }

        /// <summary>
        /// Verifies that DeleteProduct returns 204 No Content and calls the service when the product is deleted.
        /// </summary>
        [Fact]
        public async Task DeleteProduct_ReturnsNoContent_AndCallsService()
        {
            var productAgent = new Mock<IProductServiceAgent>();
            var inventoryAgent = new Mock<IInventoryServiceAgent>();
            productAgent.Setup(x => x.DeleteProductAsync(7)).Returns(Task.CompletedTask).Verifiable();

            var productService = new ProductService(productAgent.Object, inventoryAgent.Object);
            var controller = new ProductsController(productService);

            var result = await controller.DeleteProduct(7);

            Assert.IsType<NoContentResult>(result);
            productAgent.Verify(x => x.DeleteProductAsync(7), Times.Once);
        }
    }
}
