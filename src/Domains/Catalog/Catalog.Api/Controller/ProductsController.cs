using Catalog.Application.Commands;
using Catalog.Application.Handlers;
using Catalog.Application.Queries;
using Catalog.Domain.Exceptions;
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
             UpdateStockHandler updateStockHandler) : ControllerBase
    {
        private readonly CreateProductHandler _createProductHandler = createProductHandler;
        private readonly GetProductAllHandler _getProductAllHandler = getProductAllHandler;
        private readonly GetProductByIdHandler _getProductByIdHandler = getProductByIdHandler;
        private readonly UpdateProductHandler _updateProductHandler = updateProductHandler;
        private readonly DeleteProductHandler _deleteProductHandler = deleteProductHandler;
        private readonly GetProductsByCategoryHandler _getProductsByCategoryHandler = getProductsByCategoryHandler;
        private readonly UpdateStockHandler _updateStockHandler = updateStockHandler;

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] CreateProductCommand command)
        {
            try
            {
                var id = await _createProductHandler.Handle(command);
                return CreatedAtAction(nameof(GetProductById), new { id });
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<Product>>> GetProductAll()
        {
            try
            {
                var products = await _getProductAllHandler.Handle();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao obter produtos");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById([FromBody] GetProductByIdQuery query)
        {
            try
            {
                var product = await _getProductByIdHandler.Handle(query);
                if (product == null)
                {
                    return NotFound();
                }
                return Ok(product);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductCommand command)
        {
            try
            {
                var updatedProduct = await _updateProductHandler.Handle(command);
                return Ok(updatedProduct);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct([FromBody] DeleteProductCommand command)
        {
            try
            {
                await _deleteProductHandler.Handle(command);
                return NoContent();
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<ICollection<Product>>> GetProductsByCategory([FromBody] GetProductsByCategoryQuery query)
        {
            try
            {
                var products = await _getProductsByCategoryHandler.Handle(query);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao obter produtos por categoria");
            }
        }
    }

}
