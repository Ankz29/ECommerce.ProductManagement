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
    /// Products API Controller.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Retrieves all products from the system.
        /// </summary>
        [Authorize(Roles = "Admin,User")]
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            return Ok(await _productService.GetAllProductsAsync());
        }

        /// <summary>
        /// Retrieves a single product by its unique identifier.
        /// </summary>
        /// <param name="id">The product ID to look up.</param>
        /// <returns>
        /// Returns 200 OK with the product details if found, 
        /// or 404 Not Found if no product exists with the given ID.
        /// </returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();

            var dto = new ProductReadDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
            };
            return Ok(dto);
        }

        /// <summary>
        /// Creates a new product in the system.
        /// </summary>
        /// <param name="productCreateDto">The product details to create.</param>
        /// <returns>
        /// Returns 201 Created with the newly created product’s basic details.
        /// </returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductCreateDto productCreateDto)
        {
            Product product = new Product
            {
                Name = productCreateDto.Name,
                Description = productCreateDto.Description,
                Price = productCreateDto.Price,
                CategoryId = productCreateDto.CategoryId,
                InventoryId = productCreateDto.InventoryId
            };

            await _productService.AddProductAsync(product);

            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, new { product.Id, product.Name, product.Price });
        }

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="id">The product ID to update.</param>
        /// <param name="product">The updated product details.</param>
        /// <returns>
        /// Returns 204 No Content if the update succeeds, 
        /// or 400 Bad Request if the ID does not match the product object.
        /// </returns>
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            if (id != product.Id) return BadRequest();
            await _productService.UpdateProductAsync(product);
            return NoContent();
        }

        /// <summary>
        /// Deletes a product by its unique identifier.
        /// </summary>
        /// <param name="id">The product ID to delete.</param>
        /// <returns>
        /// Returns 204 No Content after successful deletion.
        /// </returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _productService.DeleteProductAsync(id);
            return NoContent();
        }
    }
}