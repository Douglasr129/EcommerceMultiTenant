using Catalog.Domain.Entities;

namespace Catalog.Domain.Interfaces
{
    public interface ICategoryRepository
    {
        Task AddCategory(Category category);
        Task<ICollection<Category>> GetCategoryAll();
        Task<Category> GetCategoryById(Guid id);
        Task<Category> UpdateCategory(Category category);
        Task DeleteCategory(Guid id);
    }
}
