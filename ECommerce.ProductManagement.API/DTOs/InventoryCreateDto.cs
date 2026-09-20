namespace ECommerce.ProductManagement.API.DTOs
{
    /// <summary>
    /// Create Inventory DTO class.
    /// </summary>
    public class InventoryCreateDto
    {
        /// <summary>
        /// Product Id
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Inventory Product Quantity
        /// </summary>
        public int Quantity { get; set; }
    }
}
