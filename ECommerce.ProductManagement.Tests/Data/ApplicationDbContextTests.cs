#region using directives

using ECommerce.ProductManagement.Core.Models;
using ECommerce.ProductManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

#endregion

namespace ECommerce.ProductManagement.Tests.Data
{
    /// <summary>
    /// Application Db Context Test class.
    /// </summary>
    public class ApplicationDbContextTests
    {
        /// <summary>
        /// Tests that the ApplicationDbContext constructor runs successfully
        /// and that the DbSet properties (Products, Categories, Inventories) are accessible.
        /// </summary>
        [Fact]
        public void Constructor_And_DbSets_AreAccessible()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new ApplicationDbContext(options);

            // Accessing DbSet properties ensures they exist and constructor ran
            Assert.NotNull(context.Products);
            Assert.NotNull(context.Categories);
            Assert.NotNull(context.Inventories);
        }

        /// <summary>
        /// Tests that OnModelCreating correctly configures the one-to-many relationship
        /// between Product and Category, including foreign key, uniqueness, and navigation properties.
        /// </summary>
        [Fact]
        public void OnModelCreating_Configures_Product_Category_OneToMany()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new ApplicationDbContext(options);

            var model = context.Model;

            var productEntity = model.FindEntityType(typeof(Product));
            var categoryEntity = model.FindEntityType(typeof(Category));

            Assert.NotNull(productEntity);
            Assert.NotNull(categoryEntity);

            var fk = productEntity.GetForeignKeys().FirstOrDefault(f => f.PrincipalEntityType.ClrType == typeof(Category));
            Assert.NotNull(fk);
            // one-to-many should not be unique
            Assert.False(fk.IsUnique);
            Assert.Contains(fk.Properties, p => p.Name == nameof(Product.CategoryId));

            // navigation properties
            var nav = productEntity.FindNavigation(nameof(Product.Category));
            Assert.NotNull(nav);

            var invNav = categoryEntity.FindNavigation(nameof(Category.Products));
            Assert.NotNull(invNav);
        }

        /// <summary>
        /// Tests that OnModelCreating correctly configures the one-to-one relationship
        /// between Product and Inventory, including foreign key, uniqueness, and navigation properties.
        /// </summary>
        [Fact]
        public void OnModelCreating_Configures_Product_Inventory_OneToOne()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new ApplicationDbContext(options);

            var model = context.Model;

            var productEntity = model.FindEntityType(typeof(Product));
            var inventoryEntity = model.FindEntityType(typeof(Inventory));

            Assert.NotNull(productEntity);
            Assert.NotNull(inventoryEntity);

            var fk = inventoryEntity.GetForeignKeys().FirstOrDefault(f => f.PrincipalEntityType.ClrType == typeof(Product));
            Assert.NotNull(fk);
            // one-to-one should be unique
            Assert.True(fk.IsUnique);
            Assert.Contains(fk.Properties, p => p.Name == nameof(Inventory.ProductId));

            var prodNav = productEntity.FindNavigation(nameof(Product.Inventory));
            Assert.NotNull(prodNav);

            var invNav = inventoryEntity.FindNavigation(nameof(Inventory.Product));
            Assert.NotNull(invNav);
        }
    }
}
