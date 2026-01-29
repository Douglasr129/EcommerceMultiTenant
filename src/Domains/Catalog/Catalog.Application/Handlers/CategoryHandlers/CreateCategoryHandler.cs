using Catalog.Application.Commands.CategoryCommands;
using Catalog.Domain.Entities;
using Catalog.Domain.Interfaces;

namespace Catalog.Application.Handlers.CategoryHandlers
{
    public class CreateCategoryHandler(ICategoryRepository categoryRepository)
    {
        private readonly ICategoryRepository _categoryRepository = categoryRepository;
        public async Task<Guid> Handle(CreateCategoryCommand command)
        {
            var category = new Category(command.Name);

            await _categoryRepository.AddCategory(category);
            return category.Id;
        }
    }
}
