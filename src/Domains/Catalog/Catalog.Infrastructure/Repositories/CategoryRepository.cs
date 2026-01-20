using Catalog.Domain.Entities;
using Catalog.Domain.Exceptions;
using Catalog.Domain.Interfaces;
using Catalog.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Repositories
{
    public class CategoryRepository(CatalogDbContext context) : ICategoryRepository
    {
        private readonly CatalogDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

        public async Task AddCategory(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategory(Guid id)
        {
            var category = await _context.Categories.FindAsync(id)
                ?? throw new DomainException("Categoria não encontrada");

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<Category>> GetCategoryAll()
        {
            return await _context.Categories
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Category> GetCategoryById(Guid id)
        {
            var category = await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id)
                ?? throw new DomainException("Categoria não encontrada");

            return category;
        }

        public async Task<Category> UpdateCategory(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            var existingCategory = await _context.Categories.FindAsync(category.Id)
                ?? throw new DomainException("Categoria não encontrada");

            existingCategory.UpdateName(category.Name);

            _context.Categories.Update(existingCategory);
            await _context.SaveChangesAsync();

            return existingCategory;
        }
    }
}
