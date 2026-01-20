using Catalog.Application.Commands;
using Catalog.Application.Handlers;
using Catalog.Application.Queries;
using Catalog.Domain.Entities;
using Catalog.Domain.Exceptions;
using Catalog.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controller
{
    [ApiController]
    [Route("api/catalog/categories")]
    public class CategoriesController(
            CreateCategoryHandler createCategoryHandler, 
            GetCategoryAllHandler getCategoryAllHandler, 
            GetCategoryByIdHandler getCategoryByIdHandler, 
            UpdateCategoryHandler updateCategoryHandler) : ControllerBase
    {
        private readonly CreateCategoryHandler _createCategoryHandler = createCategoryHandler;
        private readonly GetCategoryAllHandler _getCategoryAllHandler = getCategoryAllHandler;
        private readonly GetCategoryByIdHandler _getCategoryByIdHandler = getCategoryByIdHandler;
        private readonly UpdateCategoryHandler _updateCategoryHandler = updateCategoryHandler;

        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] CreateCategoryCommand command)
        {
            try
            {
                var id = await _createCategoryHandler.Handle(command);
                return CreatedAtAction(nameof(GetCategoryById), new { id });
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<Category>>> GetCategoryAll()
        {
            try
            {
                var categories = await _getCategoryAllHandler.Handle();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao obter categorias");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategoryById([FromBody] GetCategoryByIdQuery guery)
        {
            try
            {
                var category = await _getCategoryByIdHandler.Handle(guery);
                if (category == null)
                {
                    return NotFound();
                }
                return Ok(category);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryCommand command)
        {
            try
            {
                var updatedCategory = await _updateCategoryHandler.Handle(command);
                return Ok(updatedCategory);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

}
