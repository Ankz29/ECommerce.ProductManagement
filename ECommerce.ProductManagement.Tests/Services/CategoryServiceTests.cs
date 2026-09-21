#region using directives

using ECommerce.ProductManagement.Core.Models;
using ECommerce.ProductManagement.Core.Services;
using ECommerce.ProductManagement.Infrastructure.Repositories;
using Moq;

#endregion

namespace ECommerce.ProductManagement.Tests.Services
{
    public class CategoryServiceTests
    {
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
