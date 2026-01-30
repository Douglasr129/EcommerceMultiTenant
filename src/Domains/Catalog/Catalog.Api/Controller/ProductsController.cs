using Catalog.Api.Middlewares;
using Catalog.Api.Models;
using Catalog.Api.Services;
using Catalog.Application.Commands.ProductCommands;
using Catalog.Application.Handlers.ProductHandlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controller
{
    [ApiController]
    [Route("api/catalog/products")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    [Authorize(Roles = "Admin,Manager")]
    public class ProductsController(
            CreateProductHandler createProductHandler,
            GetProductAllHandler getProductAllHandler,
            GetProductByIdHandler getProductByIdHandler,
            UpdateProductHandler updateProductHandler,
            DeleteProductHandler deleteProductHandler,
            GetProductsByCategoryHandler getProductsByCategoryHandler,
            UpdateProductsByCategoryHandler updateProductsByCategoryHandler,
            ILinkService linkService) : ControllerBase
    {
        private readonly CreateProductHandler _createProductHandler = createProductHandler;
        private readonly GetProductAllHandler _getProductAllHandler = getProductAllHandler;
        private readonly GetProductByIdHandler _getProductByIdHandler = getProductByIdHandler;
        private readonly UpdateProductHandler _updateProductHandler = updateProductHandler;
        private readonly DeleteProductHandler _deleteProductHandler = deleteProductHandler;
        private readonly GetProductsByCategoryHandler _getProductsByCategoryHandler = getProductsByCategoryHandler;
        private readonly UpdateProductsByCategoryHandler _updateProductsByCategoryHandler = updateProductsByCategoryHandler;
        private readonly ILinkService _linkService = linkService;

        /// <summary>
        /// Cria um novo produto no catálogo
        /// </summary>
        /// <param name="command">Dados do produto a ser criado</param>
        /// <returns>Produto criado com sucesso</returns>
        /// <response code="201">Produto criado com sucesso</response>
        /// <response code="400">Dados inválidos ou categoria não encontrada</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status201Created)]
        public async Task<IActionResult> AddProduct([FromBody] CreateProductCommand command)
        {
            if (!ModelState.IsValid) return BadRequest(new { error = ModelState });

            var id = await _createProductHandler.Handle(command);
            var product = await _getProductByIdHandler.Handle(id);

            var response = ProductResponse.FromProduct(product);
            response.Links = _linkService.GenerateProductLinks(id);

            return CreatedAtAction(nameof(GetProductById), new { id = id }, response);
        }


        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
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
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] UpdateProductCommand command)
        {
            if (!ModelState.IsValid) return BadRequest(new { error = ModelState });
            if(!id.Equals(command.ProductId)) return BadRequest(new { error = "Os IDs não conferem" });
            var updatedProduct = await _updateProductHandler.Handle(command);
            return Ok(updatedProduct);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            await _deleteProductHandler.Handle(id);
            return NoContent();
        }

        [HttpGet]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
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
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<ICollection<Product>>> GetProductsByCategory(Guid categoryId)
        {
            var products = await _getProductsByCategoryHandler.Handle(categoryId);

            var response = new
            {
                Products = products.Select(p =>
                {
                    var productResponse = ProductResponse.FromProduct(p);
                    productResponse.Links = _linkService.GenerateProductByCategoryIdLinks(p.Id);
                    return productResponse;
                }),
                Links = _linkService.GenerateProductsLinks()
            };
            return Ok(response);
        }
        [HttpPut("category/{categoryId}/product/{productId}")]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<ICollection<Product>>> UpdateProductsByCategory(Guid categoryId, Guid productId)
        {
            if (!ModelState.IsValid) return BadRequest(new { error = ModelState });
            var product = await _updateProductsByCategoryHandler.Handle(categoryId, productId);
            return Ok(product);
        }
    }

}
