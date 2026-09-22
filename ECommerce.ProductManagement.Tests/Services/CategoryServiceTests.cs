#region using directives

using ECommerce.ProductManagement.Core.Models;
using ECommerce.ProductManagement.Core.Services;
using ECommerce.ProductManagement.Infrastructure.Repositories;
using Moq;

#endregion

namespace ECommerce.ProductManagement.Tests.Services
{
    /// <summary>
    /// Category Service Test class.
    /// </summary>
    public class CategoryServiceTests
    {
        /// <summary>
        /// Tests that GetAllCategoriesAsync returns the expected list of categories
        /// provided by the mocked category service agent.
        /// </summary>
        [Fact]
        public async Task GetAllCategoriesAsync_ReturnsCategories()
        {
            var list = new List<Category>
            {
                new Category { Id = 1, Name = "Cat1" },
                new Category { Id = 2, Name = "Cat2" }
            };

            var agent = new Mock<ICategoryServiceAgent>();
            agent.Setup(x => x.GetAllCategoryAsync()).ReturnsAsync(list);

            var service = new CategoryService(agent.Object);

            var result = await service.GetAllCategoriesAsync();

            Assert.Same(list, result);
        }

        /// <summary>
        /// Tests that AddCategoryAsync calls the category service agent's AddCategoryAsync method
        /// exactly once with the specified category.
        /// </summary>
        [Fact]
        public async Task AddCategoryAsync_CallsAgent()
        {
            var category = new Category { Name = "New" };

            var agent = new Mock<ICategoryServiceAgent>();
            agent.Setup(x => x.AddCategoryAsync(It.IsAny<Category>())).Returns(Task.CompletedTask).Verifiable();

            var service = new CategoryService(agent.Object);

            await service.AddCategoryAsync(category);

            agent.Verify(x => x.AddCategoryAsync(category), Times.Once);
        }

        /// <summary>
        /// Tests that UpdateCategoryAsync calls the category service agent's UpdateCategoryAsync method
        /// exactly once with the specified category.
        /// </summary>
        [Fact]
        public async Task UpdateCategoryAsync_CallsAgent()
        {
            var category = new Category { Id = 3, Name = "Updated" };

            var agent = new Mock<ICategoryServiceAgent>();
            agent.Setup(x => x.UpdateCategoryAsync(It.IsAny<Category>())).Returns(Task.CompletedTask).Verifiable();

            var service = new CategoryService(agent.Object);

            await service.UpdateCategoryAsync(category);

            agent.Verify(x => x.UpdateCategoryAsync(category), Times.Once);
        }

        /// <summary>
        /// Tests that DeleteCategoryAsync calls the category service agent's DeleteCategoryAsync method
        /// exactly once with the specified category Id.
        /// </summary>
        [Fact]
        public async Task DeleteCategoryAsync_CallsAgent()
        {
            var agent = new Mock<ICategoryServiceAgent>();
            agent.Setup(x => x.DeleteCategoryAsync(It.IsAny<int>())).Returns(Task.CompletedTask).Verifiable();

            var service = new CategoryService(agent.Object);

            await service.DeleteCategoryAsync(55);

            agent.Verify(x => x.DeleteCategoryAsync(55), Times.Once);
        }
    }
}
