using Catalog.Domain.Exceptions;
using Catalog.Domain.Interfaces;
using Catalog.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Repositories
{
    public class ProductRepository(CatalogDbContext context) : IProductRepository
    {
        private readonly CatalogDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

        public async Task AddProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));
            if (product.CategoryId is not null)
            {
                if (await ValidateCategory(product.CategoryId.Value))
                {
                    await _context.Products.AddAsync(product);
                    await _context.SaveChangesAsync();
                }
                else 
                {
                    throw new DomainException("Categoria não encontrado");
                }

            }

        }

        public async Task DeleteProduct(Guid id)
        {
            var product = await _context.Products.FindAsync(id)
                ?? throw new DomainException("Produto não encontrado");

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<Product>> GetProductAll()
        {
            return await _context.Products
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Product> GetProductById(Guid id)
        {
            var product = await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id)
                ?? throw new DomainException("Produto não encontrado");

            return product;
        }

        public async Task<ICollection<Product>> GetProductsByCategory(Guid categoryId)
        {
            return await _context.Products
                .AsNoTracking()
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            var existingProduct = await _context.Products.FindAsync(product.Id)
                ?? throw new DomainException("Produto não encontrado");
            if (!existingProduct.Name.Equals(product.Name))
            {
                existingProduct.UpdateName(product.Name);
            }
            if (!existingProduct.Price.Amount.Equals(product.Price.Amount))
            {
                existingProduct.UpdatePrice(product.Price);
            }
            if (!existingProduct.CategoryId.Equals(product.CategoryId))
            {
                if (product.CategoryId is not null)
                {
                    if (await ValidateCategory(product.CategoryId.Value))
                    {
                        existingProduct.UpdateCategory(product.CategoryId.Value);
                    }
                }
            }
            if (existingProduct.Stock != product.Stock)
            {
                existingProduct.UpdateStock(product.Stock - existingProduct.Stock);
            }

            _context.Products.Update(existingProduct);
            await _context.SaveChangesAsync();

            return existingProduct;
        }
        private async Task<bool> ValidateCategory(Guid categoryId)
        {
            var categoria = await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == categoryId);
            if (categoria != null) return true;
            return false;


        }
    }
}
