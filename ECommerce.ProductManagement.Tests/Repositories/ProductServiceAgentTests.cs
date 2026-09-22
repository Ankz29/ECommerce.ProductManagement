#region using directives

using ECommerce.ProductManagement.Core.Models;
using ECommerce.ProductManagement.Infrastructure.Data;
using ECommerce.ProductManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

#endregion

namespace ECommerce.ProductManagement.Tests.Repositories
{
    /// <summary>
    /// ProductServiceAgentTests class contains unit tests for the ProductServiceAgent class,
    /// which is responsible for managing product-related operations in the application.
    /// </summary>
    public class ProductServiceAgentTests
    {
        private static ApplicationDbContext CreateContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            return new ApplicationDbContext(options);
        }

        /// <summary>
        /// Tests that GetAllProductsAsync returns products with their associated
        /// Category and Inventory entities correctly loaded from the database.
        /// </summary>
        [Fact]
        public async Task GetAllProductsAsync_ReturnsProducts_WithCategoryAndInventory()
        {
            var dbName = Guid.NewGuid().ToString();
            await using (var context = CreateContext(dbName))
            {
                var category = new Category { Name = "C1" };
                context.Categories.Add(category);
                await context.SaveChangesAsync();

                var product = new Product { Name = "P1", Description = "D", Price = 1m, CategoryId = category.Id };
                context.Products.Add(product);
                await context.SaveChangesAsync();

                var inventory = new Inventory { ProductId = product.Id, Quantity = 5 };
                context.Inventories.Add(inventory);
                await context.SaveChangesAsync();
            }

            await using (var context = CreateContext(dbName))
            {
                var agent = new ProductServiceAgent(context);
                var list = (await agent.GetAllProductsAsync()).ToList();

                Assert.Single(list);
                var p = list[0];
                Assert.Equal("P1", p.Name);
                Assert.NotNull(p.Category);
                Assert.Equal("C1", p.Category.Name);
                Assert.NotNull(p.Inventory);
                Assert.Equal(5, p.Inventory.Quantity);
            }
        }

        /// <summary>
        /// Tests that GetProductByIdAsync returns the expected product by Id,
        /// including its associated Category and Inventory entities.
        /// </summary>
        [Fact]
        public async Task GetProductByIdAsync_ReturnsProduct_WithCategoryAndInventory()
        {
            var dbName = Guid.NewGuid().ToString();
            int productId;

            await using (var context = CreateContext(dbName))
            {
                var category = new Category { Name = "CatX" };
                context.Categories.Add(category);
                await context.SaveChangesAsync();

                var product = new Product { Name = "PX", Description = "DX", Price = 2m, CategoryId = category.Id };
                context.Products.Add(product);
                await context.SaveChangesAsync();
                productId = product.Id;

                var inventory = new Inventory { ProductId = product.Id, Quantity = 11 };
                context.Inventories.Add(inventory);
                await context.SaveChangesAsync();
            }

            await using (var context = CreateContext(dbName))
            {
                var agent = new ProductServiceAgent(context);
                var p = await agent.GetProductByIdAsync(productId);

                Assert.NotNull(p);
                Assert.Equal("PX", p.Name);
                Assert.NotNull(p.Category);
                Assert.Equal("CatX", p.Category.Name);
                Assert.NotNull(p.Inventory);
                Assert.Equal(11, p.Inventory.Quantity);
            }
        }

        /// <summary>
        /// Tests that AddProductAsync adds a new product to the database,
        /// assigns it an Id, and persists it correctly.
        /// </summary>
        [Fact]
        public async Task AddProductAsync_AddsProductAndPersists()
        {
            var dbName = Guid.NewGuid().ToString();

            await using (var context = CreateContext(dbName))
            {
                var agent = new ProductServiceAgent(context);
                var product = new Product { Name = "NewP", Description = "desc", Price = 3m };

                await agent.AddProductAsync(product);

                Assert.True(product.Id > 0);
                var fromDb = await context.Products.FindAsync(product.Id);
                Assert.NotNull(fromDb);
                Assert.Equal("NewP", fromDb.Name);
            }
        }

        /// <summary>
        /// Tests that UpdateProductAsync updates an existing product in the database
        /// and persists the changes correctly.
        /// </summary>
        [Fact]
        public async Task UpdateProductAsync_UpdatesExistingProduct()
        {
            var dbName = Guid.NewGuid().ToString();
            int id;

            await using (var context = CreateContext(dbName))
            {
                var p = new Product { Name = "Before", Description = "b", Price = 1m };
                context.Products.Add(p);
                await context.SaveChangesAsync();
                id = p.Id;
            }

            await using (var context = CreateContext(dbName))
            {
                var agent = new ProductServiceAgent(context);
                var p = await context.Products.FindAsync(id);
                p.Name = "After";
                await agent.UpdateProductAsync(p);
            }

            await using (var context = CreateContext(dbName))
            {
                var p = await context.Products.FindAsync(id);
                Assert.Equal("After", p.Name);
            }
        }

        /// <summary>
        /// Tests that DeleteProductAsync removes an existing product from the database
        /// and does not throw an error when attempting to delete a non-existent product.
        /// </summary>
        [Fact]
        public async Task DeleteProductAsync_RemovesProduct_WhenExists_And_NoError_WhenNotExists()
        {
            var dbName = Guid.NewGuid().ToString();
            int id;

            await using (var context = CreateContext(dbName))
            {
                var p = new Product { Name = "ToDelete", Description = "desc", Price = 1m };
                context.Products.Add(p);
                await context.SaveChangesAsync();
                id = p.Id;
            }

            await using (var context = CreateContext(dbName))
            {
                var agent = new ProductServiceAgent(context);
                await agent.DeleteProductAsync(id);

                var found = await context.Products.FindAsync(id);
                Assert.Null(found);

                // calling delete on non-existing id should not throw
                await agent.DeleteProductAsync(9999);
            }
        }
    }
}
