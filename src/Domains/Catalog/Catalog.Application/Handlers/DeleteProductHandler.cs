using Catalog.Application.Commands;
using Catalog.Domain.Exceptions;
using Catalog.Domain.Interfaces;
using Catalog.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Handlers
{
    public class DeleteProductHandler(IProductRepository repository)
    {
        private readonly IProductRepository _repository = repository;
        public async Task Handle(DeleteProductCommand command)
        {
            var product = await _repository.GetProductById(command.ProductId);

            if (product == null)
                throw new DomainException("Produto não encontrado");
            // Não toca no estoque aqui!

            await _repository.DeleteProduct(product.Id);
        }
    }
}
