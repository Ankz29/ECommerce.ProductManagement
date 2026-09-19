using ECommerce.ProductManagement.Core.Models;

namespace ECommerce.ProductManagement.Infrastructure.Repositories
{
    /// <summary>
    /// Interface class for Category.
    /// </summary>
    public interface ICategoryServiceAgent
    {
        /// <summary>
        /// Asynchronously retrieves all category from the data source.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation, containing a collection of category entities.
        /// </returns>
        Task<IEnumerable<Category>> GetAllAsync();

        /// <summary>
        /// Asynchronously retrieves a category by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the category.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing the category entity if found; otherwise null.
        /// </returns>
        Task<Category> GetByIdAsync(int id);

        /// <summary>
        /// Asynchronously adds a new category to the data source.
        /// </summary>
        /// <param name="category">The category entity to add.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        Task AddAsync(Category category);

        /// <summary>
        /// Asynchronously updates an existing category in the data source.
        /// </summary>
        /// <param name="category">The category entity with updated values.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        Task UpdateAsync(Category category);

        /// <summary>
        /// Asynchronously deletes a category from the data source by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the category to delete.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        Task DeleteAsync(int id);
    }
}