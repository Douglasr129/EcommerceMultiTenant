using Catalog.Application.Commands.CategoryCommands;
using Catalog.Domain.Entities;
using Catalog.Domain.Exceptions;
using Catalog.Domain.Interfaces;
using Catalog.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Handlers.CategoryHandlers
{
    public class UpdateCategoryHandler(ICategoryRepository repository)
    {
        private readonly ICategoryRepository _repository = repository;

        public async Task<Category> Handle(UpdateCategoryCommand command)
        {
            var category = await _repository.GetCategoryById(command.CategoryId);

            if (category == null)
                throw new DomainException("Categoria não encontrado");

            // Não toca no estoque aqui!
            category.UpdateName(command.Name);
            return await _repository.UpdateCategory(category);
        }
    }
}
