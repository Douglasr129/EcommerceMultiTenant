using Catalog.Application.Commands;
using Catalog.Application.Queries;
using Catalog.Domain.Entities;
using Catalog.Domain.Interfaces;
using Catalog.Domain.ValueObjects;

namespace Catalog.Application.Handlers
{
    public class CreateProductHandler(IProductRepository repository)
    {
        private readonly IProductRepository _repository = repository;

        public async Task<Guid> Handle(CreateProductCommand command)
        {
            var price = new Money(command.Price, "BRL");
            var product = new Product(
                command.Name,
                price,
                command.StockInicial,  // Obrigatório na criação
                command.CategoryId
            );

            await _repository.AddProduct(product);
            return product.Id;
        }
    }


}
