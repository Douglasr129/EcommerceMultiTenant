using Catalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task AddProduct(Product product);
        Task<ICollection<Product>> GetProductAll();
        Task<Product> GetProductById(Guid id);
        Task<Product> UpdateProduct(Product product);
        Task DeleteProduct(Guid id);
        Task<ICollection<Product>> GetProductsByCategory(Guid id);
    }
}
