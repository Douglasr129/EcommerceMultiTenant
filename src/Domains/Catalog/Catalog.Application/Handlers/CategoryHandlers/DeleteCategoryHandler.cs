using Catalog.Application.Commands;
using Catalog.Domain.Exceptions;
using Catalog.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Handlers.CategoryHandlers
{
    public class DeleteCategoryHandler(ICategoryRepository repository)
    {

        private readonly ICategoryRepository _repository = repository;
        public async Task Handle(Guid id)
        {
            var product = await _repository.GetCategoryById(id);

            if (product == null)
                throw new DomainException("Produto não encontrado");
            // Não toca no estoque aqui!

            await _repository.DeleteCategory(product.Id);
        }
    }
}
