using Catalog.Domain.Interfaces;


namespace Catalog.Application.Handlers.ProductHandlers
{
    public class GetProductByIdHandler(IProductRepository repository)
    {
        private readonly IProductRepository _repository = repository;

        public async Task<Product> Handle(Guid id)
        {
            return await _repository.GetProductById (id);
        }
    }

}
