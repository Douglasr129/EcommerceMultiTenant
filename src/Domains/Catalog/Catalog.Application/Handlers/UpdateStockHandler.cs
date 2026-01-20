using Catalog.Application.Commands;
using Catalog.Domain.Entities;
using Catalog.Domain.Exceptions;
using Catalog.Domain.Interfaces;

namespace Catalog.Application.Handlers
{
    public class UpdateStockHandler(IProductRepository repository)
    {
        private readonly IProductRepository _repository = repository;
        public async Task<Product> Handle(UpdateStockCommand command)
        {
            var product = await _repository.GetProductById(command.ProductId);

            if (product == null)
                throw new DomainException("Produto não encontrado");

            product.UpdateStock(command.amount);

            return await _repository.UpdateProduct(product);
        }
    }
}
