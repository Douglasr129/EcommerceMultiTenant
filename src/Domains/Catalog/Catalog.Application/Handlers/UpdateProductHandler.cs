using Catalog.Application.Commands;
using Catalog.Application.Queries;
using Catalog.Domain.Entities;
using Catalog.Domain.Exceptions;
using Catalog.Domain.Interfaces;
using Catalog.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Handlers
{
    public class UpdateProductHandler(IProductRepository repository)
    {
        private readonly IProductRepository _repository = repository;
        public async Task<Product> Handle(UpdateProductCommand command)
        {
            var product = await _repository.GetProductById(command.ProductId);

            if (product == null)
                throw new DomainException("Produto não encontrado");
            if (!(command.Price is null))
            {
                if (decimal.IsNegative(command.Price.Value))
                    throw new DomainException("Preço inválido");
                product.UpdatePrice(new Money(command.Price.Value, "BRL"));
            }
            // Não toca no estoque aqui!

            return await _repository.UpdateProduct(product);
        }
    }
}
