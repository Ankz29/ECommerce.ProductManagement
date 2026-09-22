using System;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.ProductManagement.Core.Models;
using ECommerce.ProductManagement.Infrastructure.Data;
using ECommerce.ProductManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECommerce.ProductManagement.Tests.Repositories
{
    public class InventoryServiceAgentTests
    {
        private static ApplicationDbContext CreateContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task GetAllInventoryAsync_Throws_InvalidOperationException_ForInvalidInclude()
        {
            var dbName = Guid.NewGuid().ToString();

            await using (var context = CreateContext(dbName))
            {
                var product = new Product { Name = "P", Description = "d", Price = 1m };
                context.Products.Add(product);
                await context.SaveChangesAsync();

                var inv = new Inventory { ProductId = product.Id, Quantity = 2 };
                context.Inventories.Add(inv);
                await context.SaveChangesAsync();
            }

            await using (var context = CreateContext(dbName))
            {
                var agent = new InventoryServiceAgent(context);
                await Assert.ThrowsAsync<InvalidOperationException>(async () => await agent.GetAllInventoryAsync());
            }
        }

        [Fact]
        public async Task GetInventoryByIdAsync_Throws_InvalidOperationException_ForInvalidInclude()
        {
            var dbName = Guid.NewGuid().ToString();
            int invId;

            await using (var context = CreateContext(dbName))
            {
                var product = new Product { Name = "P2", Description = "d2", Price = 2m };
                context.Products.Add(product);
                await context.SaveChangesAsync();

                var inv = new Inventory { ProductId = product.Id, Quantity = 3 };
                context.Inventories.Add(inv);
                await context.SaveChangesAsync();
                invId = inv.Id;
            }

            await using (var context = CreateContext(dbName))
            {
                var agent = new InventoryServiceAgent(context);
                await Assert.ThrowsAsync<InvalidOperationException>(async () => await agent.GetInventoryByIdAsync(invId));
            }
        }

        [Fact]
        public async Task AddInventoryAsync_AddsInventoryAndPersists()
        {
            var dbName = Guid.NewGuid().ToString();

            await using (var context = CreateContext(dbName))
            {
                var product = new Product { Name = "PR", Description = "desc", Price = 5m };
                context.Products.Add(product);
                await context.SaveChangesAsync();

                var agent = new InventoryServiceAgent(context);
                var inv = new Inventory { ProductId = product.Id, Quantity = 10 };

                await agent.AddInventoryAsync(inv);

                Assert.True(inv.Id > 0);
                var fromDb = await context.Inventories.FindAsync(inv.Id);
                Assert.NotNull(fromDb);
                Assert.Equal(10, fromDb.Quantity);
            }
        }

        [Fact]
        public async Task UpdateInventoryAsync_UpdatesExistingInventory()
        {
            var dbName = Guid.NewGuid().ToString();
            int id;

            await using (var context = CreateContext(dbName))
            {
                var product = new Product { Name = "PR2", Description = "desc2", Price = 6m };
                context.Products.Add(product);
                await context.SaveChangesAsync();

                var inv = new Inventory { ProductId = product.Id, Quantity = 1 };
                context.Inventories.Add(inv);
                await context.SaveChangesAsync();
                id = inv.Id;
            }

            await using (var context = CreateContext(dbName))
            {
                var agent = new InventoryServiceAgent(context);
                var inv = await context.Inventories.FindAsync(id);
                inv.Quantity = 99;
                await agent.UpdateInventoryAsync(inv);
            }

            await using (var context = CreateContext(dbName))
            {
                var inv = await context.Inventories.FindAsync(id);
                Assert.Equal(99, inv.Quantity);
            }
        }

        [Fact]
        public async Task DeleteInventoryAsync_RemovesInventory_WhenExists_And_NoError_WhenNotExists()
        {
            var dbName = Guid.NewGuid().ToString();
            int id;

            await using (var context = CreateContext(dbName))
            {
                var product = new Product { Name = "PR3", Description = "d3", Price = 7m };
                context.Products.Add(product);
                await context.SaveChangesAsync();

                var inv = new Inventory { ProductId = product.Id, Quantity = 4 };
                context.Inventories.Add(inv);
                await context.SaveChangesAsync();
                id = inv.Id;
            }

            await using (var context = CreateContext(dbName))
            {
                var agent = new InventoryServiceAgent(context);
                await agent.DeleteInventoryAsync(id);

                var found = await context.Inventories.FindAsync(id);
                Assert.Null(found);

                // calling delete on non-existing id should not throw
                await agent.DeleteInventoryAsync(9999);
            }
        }
    }
}
