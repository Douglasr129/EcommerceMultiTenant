using Catalog.Domain.Entities;
using Catalog.Domain.Interfaces;

namespace Catalog.Application.Handlers.CategoryHandlers
{
    public class GetCategoryAllHandler(ICategoryRepository repository)
    {
        private readonly ICategoryRepository _repository = repository;

        public async Task<ICollection<Category>> Handle()
        {
            return await _repository.GetCategoryAll();
        }
    }
}
