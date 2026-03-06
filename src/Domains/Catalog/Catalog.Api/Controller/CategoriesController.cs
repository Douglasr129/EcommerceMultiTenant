using Catalog.Api.Middlewares;
using Catalog.Api.Models;
using Catalog.Api.Services;
using Catalog.Application.Commands.CategoryCommands;
using Catalog.Application.Handlers.CategoryHandlers;
using Catalog.Application.Handlers.ProductHandlers;
using Catalog.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controller
{
    [ApiController]
    [Route("api/catalog/categories")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    [Authorize(Roles = "ADMIN,MANAGER")]
    [EnableCors("AngularAppPolicy")]
    public class CategoriesController(
            CreateCategoryHandler createCategoryHandler, 
            GetCategoryAllHandler getCategoryAllHandler, 
            GetCategoryByIdHandler getCategoryByIdHandler, 
            UpdateCategoryHandler updateCategoryHandler,
            DeleteCategoryHandler deleteCategoryHandler,
            ILinkService linkService) : ControllerBase
    {
        private readonly CreateCategoryHandler _createCategoryHandler = createCategoryHandler;
        private readonly GetCategoryAllHandler _getCategoryAllHandler = getCategoryAllHandler;
        private readonly GetCategoryByIdHandler _getCategoryByIdHandler = getCategoryByIdHandler;
        private readonly UpdateCategoryHandler _updateCategoryHandler = updateCategoryHandler;
        private readonly DeleteCategoryHandler _deleteCategoryHandler = deleteCategoryHandler;
        private readonly ILinkService _linkService = linkService;

        [HttpPost]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status201Created)]
        public async Task<IActionResult> AddCategory([FromBody] CreateCategoryCommand command)
        {
            if (!ModelState.IsValid) return BadRequest(new { error = ModelState });

            var id = await _createCategoryHandler.Handle(command);
            var category = await _getCategoryByIdHandler.Handle(id);

            var response = CategoryResponse.FromCategory(category);
            response.Links = _linkService.GenerateCategoryLinks(id);

            return CreatedAtAction(nameof(GetCategoryById), new { id = id }, response);
        }


        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<Category>> GetCategoryById(Guid id)
        {
            var category = await _getCategoryByIdHandler.Handle(id);
            if (category == null)
            {
                return NotFound();
            }

            var response = CategoryResponse.FromCategory(category);
            response.Links = _linkService.GenerateCategoryLinks(id);

            return Ok(response);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] UpdateCategoryCommand command)
        {
            if (!ModelState.IsValid) return BadRequest(new { error = ModelState });
            if (!id.Equals(command.CategoryId)) return BadRequest(new { error = "Os IDs não conferem" });
            var updatedCategory = await _updateCategoryHandler.Handle(command);
            return Ok(updatedCategory);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            await _deleteCategoryHandler.Handle(id);
            return NoContent();
        }

        [HttpGet]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<ICollection<Category>>> GetCategoryAll()
        {
            var Categorys = await _getCategoryAllHandler.Handle();

            var response = new
            {
                Categorys = Categorys.Select(p =>
                {
                    var categoryResponse = CategoryResponse.FromCategory(p);
                    categoryResponse.Links = _linkService.GenerateCategoryLinks(p.Id);
                    return categoryResponse;
                }),
                Links = _linkService.GenerateCategorysLinks()
            };

            return Ok(response);
        }

    }

}
