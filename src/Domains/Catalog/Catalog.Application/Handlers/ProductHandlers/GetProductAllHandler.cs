using Catalog.Domain.Interfaces;
using Catalog.Domain.Records;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.Handlers.ProductHandlers
{
    public class GetProductAllHandler(IProductRepository repository)
    {
        private readonly IProductRepository _repository = repository;

        public async Task<(IEnumerable<Product>, int)> Handle(ProductFilters filters)
        {
            return await _repository.GetProductAll(filters);
        }
    }
}
