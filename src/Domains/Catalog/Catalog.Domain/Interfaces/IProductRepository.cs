using Catalog.Domain.Records;

namespace Catalog.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task AddProduct(Product product);
        Task<(IEnumerable<Product>, int)> GetProductAll(ProductFilters filters);
        Task<Product> GetProductById(Guid id);
        Task<Product> UpdateProduct(Product product);
        Task DeleteProduct(Guid id);
        Task<ICollection<Product>> GetProductsByCategory(Guid id);
    }
}
