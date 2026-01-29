using Catalog.Domain.Exceptions;
using Catalog.Domain.Interfaces;
namespace Catalog.Application.Handlers.ProductHandlers
{
    public class UpdateProductsByCategoryHandler(IProductRepository repository)
    {
        private readonly IProductRepository _repository = repository;

        public async Task<Product> Handle(Guid categoryId, Guid productId)
        {
            var product = await _repository.GetProductById(productId);
            if (product == null)
                throw new DomainException("Produto não encontrado");
            product.UpdateCategory(categoryId);
            return await _repository.UpdateProduct(product);
        }
    }
    
}
