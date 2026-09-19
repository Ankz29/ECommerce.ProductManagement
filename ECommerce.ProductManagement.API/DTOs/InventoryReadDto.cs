namespace ECommerce.ProductManagement.API.DTOs
{
    public class InventoryReadDto
    {
        /// <summary>
        /// Inventory Id
        /// </summary>
        public int Id { get; set; }

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
