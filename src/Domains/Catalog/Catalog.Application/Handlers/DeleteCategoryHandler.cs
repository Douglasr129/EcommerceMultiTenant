using Catalog.Application.Commands;
using Catalog.Domain.Exceptions;
using Catalog.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Handlers
{
    public class DeleteCategoryHandler(ICategoryRepository repository)
    {

        private readonly ICategoryRepository _repository = repository;
        public async Task Handle(DeleteProductCommand command)
        {
            var product = await _repository.GetCategoryById(command.ProductId);

            if (product == null)
                throw new DomainException("Produto não encontrado");
            // Não toca no estoque aqui!

            await _repository.DeleteCategory(product.Id);
        }
    }
}
