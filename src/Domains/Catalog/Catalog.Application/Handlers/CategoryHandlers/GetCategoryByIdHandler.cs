using Catalog.Domain.Entities;
using Catalog.Domain.Interfaces;
namespace Catalog.Application.Handlers.CategoryHandlers
{
    public class GetCategoryByIdHandler(ICategoryRepository repository)
    {
        private readonly ICategoryRepository _repository = repository;

        public async Task<Category> Handle(Guid id)
        {
            return await _repository.GetCategoryById(id);
        }
    }
}
