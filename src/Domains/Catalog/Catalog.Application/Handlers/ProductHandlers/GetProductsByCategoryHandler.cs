using Catalog.Domain.Interfaces;


namespace Catalog.Application.Handlers.ProductHandlers
{
    public class GetProductsByCategoryHandler(IProductRepository repository)
    {
        private readonly IProductRepository _repository = repository;

        public async Task<ICollection<Product>> Handle(Guid id)
        {
            return await _repository.GetProductsByCategory(id);
        }
    }
}
