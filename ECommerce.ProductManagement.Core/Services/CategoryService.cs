using ECommerce.ProductManagement.Core.Models;
using ECommerce.ProductManagement.Infrastructure.Repositories;

namespace ECommerce.ProductManagement.Core.Services
{
    public class CategoryService
    {
        private readonly ICategoryServiceAgent _categoryServiceAgent;

        public CategoryService(ICategoryServiceAgent categoryServiceAgent)
        {
            _categoryServiceAgent = categoryServiceAgent;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync() =>
            await _categoryServiceAgent.GetAllAsync();

        public async Task AddCategoryAsync(Category category) =>
            await _categoryServiceAgent.AddAsync(category);

        public async Task UpdateCategoryAsync(Category category) =>
            await _categoryServiceAgent.UpdateAsync(category);

        public async Task DeleteCategoryAsync(int id) =>
            await _categoryServiceAgent.DeleteAsync(id);
    }
}
