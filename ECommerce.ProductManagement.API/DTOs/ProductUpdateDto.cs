using System.ComponentModel.DataAnnotations;

namespace ECommerce.ProductManagement.API.DTOs
{
    /// <summary>
    /// Update Product DTO class.
    /// </summary>
    public class ProductUpdateDto
    {
        /// <summary>
        /// Product Id
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Product Name
        /// </summary>
        [Required]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Product Description
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Product Price
        /// </summary>
        [Required]
        public decimal Price { get; set; }

        /// <summary>
        /// Product belongs to which CategoryId
        /// </summary>
        [Required]
        public int CategoryId { get; set; }

        /// <summary>
        /// Product belongs to which InventoryId
        /// </summary>
        [Required]
        public int InventoryId { get; set; }
    }
}
