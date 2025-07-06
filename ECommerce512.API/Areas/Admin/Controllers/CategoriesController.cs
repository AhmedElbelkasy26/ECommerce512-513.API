using Microsoft.AspNetCore.Mvc;
using Mapster;

namespace ECommerce512.API.Areas.Admin.Controllers
{
    [Route("api/[area]/[controller]")]
    [Area("Admin")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryRepository.GetAsync();

            //var categoryResponses = categories.Select(e => new CategoryResponse()
            //{
            //    Id = e.Id,
            //    Description = e.Description,
            //    Name = e.Name,
            //    Status = e.Status,
            //});
            
            return Ok(categories.Adapt<IEnumerable<CategoryResponse>>());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne([FromRoute] int id)
        {
            var category = await _categoryRepository.GetOneAsync(e => e.Id == id);

            if (category is not null)
            {
                //var categoryResponse = new CategoryResponse()
                //{
                //    Id = category.Id,
                //    Description = category.Description,
                //    Name = category.Name,
                //    Status = category.Status,
                //};
                
                return Ok(category.Adapt<CategoryResponse>());
            }

            return NotFound();
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] CategoryRequest categoryRequest)
        {
            var category = await _categoryRepository.CreateAsync(categoryRequest.Adapt<Category>());
            await _categoryRepository.CommitAsync();

            if(category is not null)
            {
                return Created($"{Request.Scheme}://{Request.Host}/api/Admin/Categories/{category.Id}", category.Adapt<CategoryResponse>());
            }

            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] CategoryRequest categoryRequest)
        {
            var category = _categoryRepository.Update(categoryRequest.Adapt<Category>());
            await _categoryRepository.CommitAsync();

            if (category is not null)
            {
                return NoContent();
            }

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var category = await _categoryRepository.GetOneAsync(e => e.Id == id);

            if (category is not null)
            {
                var result = _categoryRepository.Delete(category);
                await _categoryRepository.CommitAsync();

                if(result)
                {
                    return NoContent();
                }

                return BadRequest();
            }

            return NotFound();
        }
    }
}
