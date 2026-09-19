using System.Text.Json.Serialization;

namespace ECommerce.ProductManagement.Core.Models
{
    /// <summary>
    /// Inventory class that defines all the necessary properties.
    /// </summary>
    public class Inventory
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

        /// <summary>
        /// Product Class
        /// </summary>
        [JsonIgnore]
        public Product Product { get; set; }
    }
}
