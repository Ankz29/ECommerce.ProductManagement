#region using directives

using ECommerce.ProductManagement.Core.Models;
using ECommerce.ProductManagement.Infrastructure.Repositories;

#endregion

namespace ECommerce.ProductManagement.Core.Services
{
    /// <summary>
    /// Category Service class.
    /// </summary>
    public class CategoryService
    {
        private readonly ICategoryServiceAgent _categoryServiceAgent;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryService"/> class.
        /// </summary>
        /// <param name="categoryServiceAgent">
        /// The service agent responsible for interacting with the data source for categories.
        /// </param>
        public CategoryService(ICategoryServiceAgent categoryServiceAgent)
        {
            _categoryServiceAgent = categoryServiceAgent;
        }

        /// <summary>
        /// Retrieves all categories from the data source.
        /// </summary>
        /// <returns>
        /// A collection of <see cref="Category"/> objects representing all categories.
        /// </returns>
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync() =>
            await _categoryServiceAgent.GetAllCategoryAsync();

        /// <summary>
        /// Adds a new category to the data source.
        /// </summary>
        /// <param name="category">The <see cref="Category"/> object to add.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        public async Task AddCategoryAsync(Category category) =>
            await _categoryServiceAgent.AddCategoryAsync(category);

        /// <summary>
        /// Updates an existing category in the data source.
        /// </summary>
        /// <param name="category">The <see cref="Category"/> object containing updated details.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        public async Task UpdateCategoryAsync(Category category) =>
            await _categoryServiceAgent.UpdateCategoryAsync(category);

        /// <summary>
        /// Deletes a category from the data source by its unique identifier.
        /// </summary>
        /// <param name="id">The ID of the category to delete.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        public async Task DeleteCategoryAsync(int id) =>
            await _categoryServiceAgent.DeleteCategoryAsync(id);
    }
}
