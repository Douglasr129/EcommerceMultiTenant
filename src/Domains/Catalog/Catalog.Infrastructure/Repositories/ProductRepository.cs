using Catalog.Domain.Exceptions;
using Catalog.Domain.Interfaces;
using Catalog.Domain.Records;
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
            else
            {
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
            }

        }

        public async Task DeleteProduct(Guid id)
        {
            var product = await _context.Products.FindAsync(id)
                ?? throw new DomainException("Produto não encontrado");

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<(IEnumerable<Product>, int)> GetProductAll(ProductFilters filters)
        {
            var query = _context.Products.AsQueryable();
            // Filtros
            if (!string.IsNullOrEmpty(filters.SearchTerm))
                query = query.Where(p => p.Name.Contains(filters.SearchTerm));

            if (!filters.ShowInactive)
                query = query.Where(p => p.Active);

            // Ordenação Dinâmica
            query = filters.Order == "asc"
                ? query.OrderBy(p => EF.Property<object>(p, filters.Sort))
                : query.OrderByDescending(p => EF.Property<object>(p, filters.Sort));

            var total = await query.CountAsync();

            var data = await query
                .Skip((filters.Page - 1) * filters.PageSize)
                .Take(filters.PageSize)
                .ToListAsync();

            return (data, total);
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
