namespace ECommerce.ProductManagement.API.DTOs
{
    /// <summary>
    /// Update Category DTO class.
    /// </summary>
    public class CategoryUpdateDto
    {
        /// <summary>
        /// Category Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Catgeory Name
        /// </summary>
        public string Name { get; set; }
    }
}
