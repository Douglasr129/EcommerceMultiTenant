using Catalog.Application.Queries;
using Catalog.Domain.Interfaces;


namespace Catalog.Application.Handlers
{
    public class GetProductsByCategoryHandler(IProductRepository repository)
    {
        private readonly IProductRepository _repository = repository;

        public async Task<ICollection<Product>> Handle(GetProductsByCategoryQuery query)
        {
            return await _repository.GetProductsByCategory(query.CategoryId);
        }
    }
}
