using System.ComponentModel.DataAnnotations;

namespace ECommerce.ProductManagement.API.Models
{
    public class ProductCreateDto
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int InventoryId { get; set; }
    }
}
