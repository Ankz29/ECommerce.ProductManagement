using ECommerce.ProductManagement.Core.Models;

namespace ECommerce.ProductManagement.Core.Interfaces
{
    public interface IProductService
    {
        Task AddProductAsync(Product product);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int id);
    }
}
