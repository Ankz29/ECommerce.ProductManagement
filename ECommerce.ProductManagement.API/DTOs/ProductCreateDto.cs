namespace ECommerce.ProductManagement.API.DTOs
{
    public class ProductCreateDto
    {
        /// <summary>
        /// Product Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Product Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Product Price
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Product belongs to which CategoryId
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Product belongs to which InventoryId
        /// </summary>
        public int InventoryId { get; set; }
    }
}
