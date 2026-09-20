using ECommerce.ProductManagement.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ProductManagement.Infrastructure.Data
{
    /// <summary>
    /// Application Db Context class for Entity framework code- database migrations.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Application DbContext class
        /// </summary>
        /// <param name="options"></param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        /// <summary>
        /// Products DbSet
        /// </summary>
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// Categories DbSet
        /// </summary>
        public DbSet<Category> Categories { get; set; }

        /// <summary>
        /// Inventories DbSet
        /// </summary>
        public DbSet<Inventory> Inventories { get; set; }

        /// <summary>
        /// Configures entity relationships for the ECommerce database context.
        /// - Product → Category: One-to-many relationship.
        ///   Each Product belongs to one Category, and each Category can have many Products.
        /// - Product → Inventory: One-to-one relationship.
        ///   Each Product has one Inventory record, and each Inventory entry corresponds to one Product.
        /// </summary>
        /// <param name="modelBuilder">
        /// The ModelBuilder instance used to configure entity mappings and relationships.
        /// </param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Product → Category (one-to-many)
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            // Product → Inventory (one-to-one)
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Inventory)
                .WithOne(i => i.Product)
                .HasForeignKey<Inventory>(i => i.ProductId);
        }
    }
}
