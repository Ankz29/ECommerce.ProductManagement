using System.Text.Json.Serialization;

namespace ECommerce.ProductManagement.Core.Models
{
    /// <summary>
    /// Product class that defines all the necessary properties.
    /// </summary>
    public class Product
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

        /// <summary>
        /// Category Class
        /// </summary>
        [JsonIgnore] // prevents circular reference in Swagger & responses
        public Category? Category { get; set; }

        /// <summary>
        /// Inventory Class
        /// </summary>
        [JsonIgnore] // prevents circular reference in Swagger & responses
        public Inventory? Inventory { get; set; }
    }
}