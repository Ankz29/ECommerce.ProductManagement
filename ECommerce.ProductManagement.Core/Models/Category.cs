using System.Text.Json.Serialization;

namespace ECommerce.ProductManagement.Core.Models
{
    /// <summary>
    /// Catgeory class that defines all the necessary properties.
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Category Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Catgeory Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Collection of Products
        /// </summary>
        [JsonIgnore]
        public ICollection<Product> Products { get; set; }
    }
}