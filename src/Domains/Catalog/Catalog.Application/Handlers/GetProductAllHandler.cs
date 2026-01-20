using Catalog.Domain.Interfaces;

namespace Catalog.Application.Handlers
{
    public class GetProductAllHandler(IProductRepository repository)
    {
        private readonly IProductRepository _repository = repository;

        public async Task<ICollection<Product>> Handle()
        {
            return await _repository.GetProductAll();
        }
    }
}
