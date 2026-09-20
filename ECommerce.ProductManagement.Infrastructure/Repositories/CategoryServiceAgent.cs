#region using directives

using ECommerce.ProductManagement.Core.Models;
using ECommerce.ProductManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

#endregion

namespace ECommerce.ProductManagement.Infrastructure.Repositories
{
    /// <summary>
    /// Category Implementation service agent class.
    /// </summary>
    public class CategoryServiceAgent :ICategoryServiceAgent
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the CategoryServiceAgent with the specified database context.
        /// </summary>
        /// <param name="context">The ApplicationDbContext used for database operations.</param>
        public CategoryServiceAgent(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Asynchronously retrieves all category, including their associated Category and Inventory details.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation, containing a collection of Category entities.
        /// </returns>
        public async Task<IEnumerable<Category>> GetAllCategoryAsync() =>
            await _context.Categories.Include(p => p.Products).Include(p => p.Id).ToListAsync();

        /// <summary>
        /// Asynchronously retrieves a category by its unique identifier, including Category and Inventory details.
        /// </summary>
        /// <param name="id">The unique identifier of the category.</param>
        /// <returns>
        /// A task representing the asynchronous operation, containing the Category entity if found; otherwise null.
        /// </returns>
        public async Task<Category> GetCategoryByIdAsync(int id) =>
            await _context.Categories.Include(p => p.Products).Include(p => p.Id)
                                   .FirstOrDefaultAsync(p => p.Id == id);

        /// <summary>
        /// Asynchronously adds a new category to the database.
        /// </summary>
        /// <param name="category">The Category entity to add.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        public async Task AddCategoryAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously updates an existing category in the database.
        /// </summary>
        /// <param name="category">The Category entity with updated values.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        public async Task UpdateCategoryAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously deletes a category from the database by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the category to delete.</param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// </returns>
        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
    }
}
