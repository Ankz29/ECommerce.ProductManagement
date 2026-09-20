using ECommerce.ProductManagement.Core.Models;

namespace ECommerce.ProductManagement.Infrastructure.Repositories
{
    /// <summary>
    /// Interface class for Product.
    /// </summary>
    public interface IProductServiceAgent
    {
            /// <summary>
            /// Asynchronously retrieves all products from the data source.
            /// </summary>
            /// <returns>
            /// A task representing the asynchronous operation, containing a collection of Product entities.
            /// </returns>
            Task<IEnumerable<Product>> GetAllProductsAsync();

            /// <summary>
            /// Asynchronously retrieves a product by its unique identifier.
            /// </summary>
            /// <param name="id">The unique identifier of the product.</param>
            /// <returns>
            /// A task representing the asynchronous operation, containing the Product entity if found; otherwise null.
            /// </returns>
            Task<Product> GetProductByIdAsync(int id);

            /// <summary>
            /// Asynchronously adds a new product to the data source.
            /// </summary>
            /// <param name="product">The Product entity to add.</param>
            /// <returns>
            /// A task representing the asynchronous operation.
            /// </returns>
            Task AddProductAsync(Product product);

            /// <summary>
            /// Asynchronously updates an existing product in the data source.
            /// </summary>
            /// <param name="product">The Product entity with updated values.</param>
            /// <returns>
            /// A task representing the asynchronous operation.
            /// </returns>
            Task UpdateProductAsync(Product product);

            /// <summary>
            /// Asynchronously deletes a product from the data source by its unique identifier.
            /// </summary>
            /// <param name="id">The unique identifier of the product to delete.</param>
            /// <returns>
            /// A task representing the asynchronous operation.
            /// </returns>
            Task DeleteProductAsync(int id);
    }
}