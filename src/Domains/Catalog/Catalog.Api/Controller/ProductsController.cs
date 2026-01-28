using Catalog.Api.Models;
using Catalog.Api.Services;
using Catalog.Application.Commands;
using Catalog.Application.Handlers;
using Catalog.Application.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controller
{
    [ApiController]
    [Route("api/catalog/products")]
    public class ProductsController(
            CreateProductHandler createProductHandler,
            GetProductAllHandler getProductAllHandler,
            GetProductByIdHandler getProductByIdHandler,
            UpdateProductHandler updateProductHandler,
            DeleteProductHandler deleteProductHandler,
            GetProductsByCategoryHandler getProductsByCategoryHandler,
            ILinkService linkService) : ControllerBase
    {
        private readonly CreateProductHandler _createProductHandler = createProductHandler;
        private readonly GetProductAllHandler _getProductAllHandler = getProductAllHandler;
        private readonly GetProductByIdHandler _getProductByIdHandler = getProductByIdHandler;
        private readonly UpdateProductHandler _updateProductHandler = updateProductHandler;
        private readonly DeleteProductHandler _deleteProductHandler = deleteProductHandler;
        private readonly GetProductsByCategoryHandler _getProductsByCategoryHandler = getProductsByCategoryHandler;
        private readonly ILinkService _linkService = linkService;

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] CreateProductCommand command)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var id = await _createProductHandler.Handle(command);
            var product = await _getProductByIdHandler.Handle(id);

            var response = ProductResponse.FromProduct(product);
            response.Links = _linkService.GenerateProductLinks(id);

            return CreatedAtAction(nameof(GetProductById), new { id = id }, response);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(Guid id)
        {
            var product = await _getProductByIdHandler.Handle(id);
            if (product == null)
            {
                return NotFound();
            }

            var response = ProductResponse.FromProduct(product);
            response.Links = _linkService.GenerateProductLinks(id);

            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductCommand command)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var updatedProduct = await _updateProductHandler.Handle(command);
            return Ok(updatedProduct);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(DeleteProductCommand command)
        {
            await _deleteProductHandler.Handle(command);
            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<Product>>> GetProductAll()
        {
            var products = await _getProductAllHandler.Handle();

            var response = new
            {
                Products = products.Select(p =>
                {
                    var productResponse = ProductResponse.FromProduct(p);
                    productResponse.Links = _linkService.GenerateProductLinks(p.Id);
                    return productResponse;
                }),
                Links = _linkService.GenerateProductsLinks()
            };

            return Ok(response);
        }

        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<ICollection<Product>>> GetProductsByCategory([FromBody] GetProductsByCategoryQuery query)
        {
            var products = await _getProductsByCategoryHandler.Handle(query);

            var response = new
            {
                Products = products.Select(p =>
                {
                    var productResponse = ProductResponse.FromProduct(p);
                    productResponse.Links = _linkService.GenerateProductLinks(p.Id);
                    return productResponse;
                }),
                Links = _linkService.GenerateProductsLinks()
            };
            return Ok(response);
        }
    }

}
