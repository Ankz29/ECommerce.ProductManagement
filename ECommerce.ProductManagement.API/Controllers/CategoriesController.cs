#region using directives

using ECommerce.ProductManagement.API.DTOs;
using ECommerce.ProductManagement.Core.Models;
using ECommerce.ProductManagement.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace ECommerce.ProductManagement.API.Controllers
{
    /// <summary>
    /// Categories API Controller.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        public CategoriesController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Retrieves all categories from the system.
        /// </summary>
        /// <returns>
        /// Returns 200 OK with the list of categories.
        /// </returns>
        [Authorize(Roles = "Admin,User")]
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        /// <summary>
        /// Creates a new category.
        /// </summary>
        /// <param name="dto">The category details to create.</param>
        /// <returns>
        /// Returns 201 Created with the newly created category’s details.
        /// </returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryCreateDto dto)
        {
            var category = new Category { Name = dto.Name };
            await _categoryService.AddCategoryAsync(category);

            var readDto = new CategoryReadDto
            {
                Id = category.Id,
                Name = category.Name
            };

            return CreatedAtAction(nameof(GetAllCategories), new { id = category.Id }, readDto);
        }

        /// <summary>
        /// Updates an existing category.
        /// </summary>
        /// <param name="id">The category ID to update.</param>
        /// <param name="category">The updated category details.</param>
        /// <returns>
        /// Returns 204 No Content if the update succeeds, 
        /// or 400 Bad Request if the ID does not match the category object.
        /// </returns>
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, Category category)
        {
            if (id != category.Id)
                return BadRequest("Category ID mismatch.");

            await _categoryService.UpdateCategoryAsync(category);
            return NoContent();
        }

        /// <summary>
        /// Deletes a category by its unique identifier.
        /// </summary>
        /// <param name="id">The category ID to delete.</param>
        /// <returns>
        /// Returns 204 No Content after successful deletion.
        /// </returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _categoryService.DeleteCategoryAsync(id);
            return NoContent();
        }
    }
}
