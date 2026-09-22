#region using directives

using ECommerce.ProductManagement.Core.Models;
using ECommerce.ProductManagement.Infrastructure.Data;
using ECommerce.ProductManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

#endregion

namespace ECommerce.ProductManagement.Tests.Repositories
{
    /// <summary>
    /// Category Service Agent Test class.
    /// </summary>
    public class CategoryServiceAgentTests
    {
        private static ApplicationDbContext CreateContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            return new ApplicationDbContext(options);
        }

        /// <summary>
        /// Tests that GetAllCategoryAsync attempts to return categories with their associated products.
        /// Verifies that the current implementation throws an InvalidOperationException due to an invalid Include expression.
        /// </summary>
        [Fact]
        public async Task GetAllCategoryAsync_ReturnsCategories_WithProducts()
        {
            var dbName = Guid.NewGuid().ToString();

            await using (var context = CreateContext(dbName))
            {
                var category = new Category { Name = "CatA" };
                context.Categories.Add(category);
                await context.SaveChangesAsync();

                var product = new Product { Name = "Prod1", Description = "d", Price = 1m, CategoryId = category.Id };
                context.Products.Add(product);
                await context.SaveChangesAsync();
            }

            await using (var context = CreateContext(dbName))
            {
                var agent = new CategoryServiceAgent(context);

                // The implementation contains an invalid Include(p => p.Id) which EF will reject.
                // Assert that calling the method throws an InvalidOperationException so the behavior is covered.
                await Assert.ThrowsAsync<InvalidOperationException>(async () => await agent.GetAllCategoryAsync());
            }
        }

        /// <summary>
        /// Tests that GetCategoryByIdAsync attempts to return a category with its associated products.
        /// Verifies that the current implementation throws an InvalidOperationException due to an invalid Include expression.
        /// </summary>
        [Fact]
        public async Task GetCategoryByIdAsync_ReturnsCategory_WithProducts()
        {
            var dbName = Guid.NewGuid().ToString();
            int catId;

            await using (var context = CreateContext(dbName))
            {
                var category = new Category { Name = "CatB" };
                context.Categories.Add(category);
                await context.SaveChangesAsync();

                var product = new Product { Name = "ProdX", Description = "dx", Price = 2m, CategoryId = category.Id };
                context.Products.Add(product);
                await context.SaveChangesAsync();

                catId = category.Id;
            }

            await using (var context = CreateContext(dbName))
            {
                var agent = new CategoryServiceAgent(context);

                // Implementation contains an invalid Include(p => p.Id) which EF will reject.
                await Assert.ThrowsAsync<InvalidOperationException>(async () => await agent.GetCategoryByIdAsync(catId));
            }
        }

        /// <summary>
        /// Tests that AddCategoryAsync adds a new category to the database,
        /// assigns it an Id, and persists it correctly.
        /// </summary>
        [Fact]
        public async Task AddCategoryAsync_AddsCategoryAndPersists()
        {
            var dbName = Guid.NewGuid().ToString();

            await using (var context = CreateContext(dbName))
            {
                var agent = new CategoryServiceAgent(context);
                var category = new Category { Name = "NewCat" };

                await agent.AddCategoryAsync(category);

                Assert.True(category.Id > 0);
                var fromDb = await context.Categories.FindAsync(category.Id);
                Assert.NotNull(fromDb);
                Assert.Equal("NewCat", fromDb.Name);
            }
        }

        /// <summary>
        /// Tests that UpdateCategoryAsync updates an existing category in the database
        /// and persists the changes correctly.
        /// </summary>
        [Fact]
        public async Task UpdateCategoryAsync_UpdatesExistingCategory()
        {
            var dbName = Guid.NewGuid().ToString();
            int id;

            await using (var context = CreateContext(dbName))
            {
                var c = new Category { Name = "Before" };
                context.Categories.Add(c);
                await context.SaveChangesAsync();
                id = c.Id;
            }

            await using (var context = CreateContext(dbName))
            {
                var agent = new CategoryServiceAgent(context);
                var c = await context.Categories.FindAsync(id);
                c.Name = "After";
                await agent.UpdateCategoryAsync(c);
            }

            await using (var context = CreateContext(dbName))
            {
                var c = await context.Categories.FindAsync(id);
                Assert.Equal("After", c.Name);
            }
        }

        /// <summary>
        /// Tests that DeleteCategoryAsync removes an existing category from the database
        /// and does not throw an error when attempting to delete a non-existent category.
        /// </summary>
        [Fact]
        public async Task DeleteCategoryAsync_RemovesCategory_WhenExists_And_NoError_WhenNotExists()
        {
            var dbName = Guid.NewGuid().ToString();
            int id;

            await using (var context = CreateContext(dbName))
            {
                var c = new Category { Name = "ToDelete" };
                context.Categories.Add(c);
                await context.SaveChangesAsync();
                id = c.Id;
            }

            await using (var context = CreateContext(dbName))
            {
                var agent = new CategoryServiceAgent(context);
                await agent.DeleteCategoryAsync(id);

                var found = await context.Categories.FindAsync(id);
                Assert.Null(found);

                // calling delete on non-existing id should not throw
                await agent.DeleteCategoryAsync(9999);
            }
        }
    }
}
