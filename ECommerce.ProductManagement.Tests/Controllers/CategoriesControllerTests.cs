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
    public class CategoriesControllerTests
    {
        /// <summary>
        /// Tests that GetAllCategories returns an OkObjectResult containing the expected list of categories.
        /// </summary>
        [Fact]
        public async Task GetAllCategories_ReturnsOk_WithCategories()
        {
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "CatA" },
                new Category { Id = 2, Name = "CatB" }
            };

            var categoryAgent = new Mock<ICategoryServiceAgent>();
            categoryAgent.Setup(x => x.GetAllCategoryAsync()).ReturnsAsync(categories);

            var categoryService = new CategoryService(categoryAgent.Object);
            var controller = new CategoriesController(categoryService);

            var result = await controller.GetAllCategories();

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Same(categories, ok.Value);
        }

        /// <summary>
        /// Tests that CreateCategory returns a CreatedAtActionResult with the correct route values,
        /// action name, and a CategoryReadDto containing the newly assigned Id and Name.
        /// </summary>
        [Fact]
        public async Task CreateCategory_ReturnsCreated_WithLocationAndBody()
        {
            var create = new CategoryCreateDto { Name = "NewCat" };

            var categoryAgent = new Mock<ICategoryServiceAgent>();

            // Simulate repository setting the Id when adding
            categoryAgent.Setup(x => x.AddCategoryAsync(It.IsAny<Category>())).Returns<Category>(c =>
            {
                c.Id = 99;
                return Task.CompletedTask;
            });

            var categoryService = new CategoryService(categoryAgent.Object);
            var controller = new CategoriesController(categoryService);

            var result = await controller.CreateCategory(create);

            var created = Assert.IsType<CreatedAtActionResult>(result);
            var dto = Assert.IsType<CategoryReadDto>(created.Value);

            Assert.Equal(nameof(CategoriesController.GetAllCategories), created.ActionName);
            Assert.Equal(99, dto.Id);
            Assert.Equal("NewCat", dto.Name);
            Assert.Equal(99, created.RouteValues!["id"]);
        }

        /// <summary>
        /// Tests that UpdateCategory returns a BadRequestObjectResult when the provided Id
        /// does not match the category's Id.
        /// </summary>
        [Fact]
        public async Task UpdateCategory_ReturnsBadRequest_WhenIdMismatch()
        {
            var categoryAgent = new Mock<ICategoryServiceAgent>();
            var categoryService = new CategoryService(categoryAgent.Object);
            var controller = new CategoriesController(categoryService);

            var category = new Category { Id = 2, Name = "X" };

            var result = await controller.UpdateCategory(1, category);

            var bad = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Category ID mismatch.", bad.Value);
        }

        /// <summary>
        /// Tests that UpdateCategory returns a NoContentResult when the update succeeds,
        /// and verifies that the service agent's UpdateCategoryAsync method is called once with the correct category.
        /// </summary>
        [Fact]
        public async Task UpdateCategory_ReturnsNoContent_WhenSuccess()
        {
            var categoryAgent = new Mock<ICategoryServiceAgent>();
            categoryAgent.Setup(x => x.UpdateCategoryAsync(It.IsAny<Category>())).Returns(Task.CompletedTask).Verifiable();

            var categoryService = new CategoryService(categoryAgent.Object);
            var controller = new CategoriesController(categoryService);

            var category = new Category { Id = 5, Name = "Updated" };

            var result = await controller.UpdateCategory(5, category);

            Assert.IsType<NoContentResult>(result);
            categoryAgent.Verify(x => x.UpdateCategoryAsync(It.Is<Category>(c => c.Id == 5 && c.Name == "Updated")), Times.Once);
        }

        /// <summary>
        /// Tests that DeleteCategory returns a NoContentResult and verifies that the service agent's
        /// DeleteCategoryAsync method is called with the specified Id.
        /// </summary>
        [Fact]
        public async Task DeleteCategory_ReturnsNoContent_AndCallsService()
        {
            var categoryAgent = new Mock<ICategoryServiceAgent>();
            categoryAgent.Setup(x => x.DeleteCategoryAsync(It.IsAny<int>())).Returns(Task.CompletedTask).Verifiable();

            var categoryService = new CategoryService(categoryAgent.Object);
            var controller = new CategoriesController(categoryService);

            var result = await controller.DeleteCategory(7);

            Assert.IsType<NoContentResult>(result);
            categoryAgent.Verify(x => x.DeleteCategoryAsync(7), Times.Once);
        }
    }
}
