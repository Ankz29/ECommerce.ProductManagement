namespace ECommerce.ProductManagement.API.DTOs
{
    public class InventoryUpdateDto
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
