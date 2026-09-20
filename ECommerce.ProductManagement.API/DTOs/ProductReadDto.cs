namespace ECommerce.ProductManagement.API.DTOs
{
    /// <summary>
    /// Read Product DTO class.
    /// </summary>
    public class ProductReadDto
    {
        /// <summary>
        /// Product Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Product Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Product Price
        /// </summary>
        public decimal Price { get; set; }
    }
}
