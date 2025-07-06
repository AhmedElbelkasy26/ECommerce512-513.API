using Microsoft.AspNetCore.Mvc;
using Mapster;
using Microsoft.AspNetCore.Authorization;

namespace ECommerce512.API.Areas.Admin.Controllers
{
    [Route("api/[area]/[controller]")]
    [Area("Admin")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandRepository _brandRepository;

        public BrandsController(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }
        [Authorize]
        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            var brands = await _brandRepository.GetAsync();

            //var brandResponses = brands.Select(e => new BrandResponse()
            //{
            //    Id = e.Id,
            //    Description = e.Description,
            //    Name = e.Name,
            //    Status = e.Status,
            //});
            
            return Ok(brands.Adapt<IEnumerable<BrandResponse>>());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne([FromRoute] int id)
        {
            var brand = await _brandRepository.GetOneAsync(e => e.Id == id);

            if (brand is not null)
            {
                //var brandResponse = new BrandResponse()
                //{
                //    Id = brand.Id,
                //    Description = brand.Description,
                //    Name = brand.Name,
                //    Status = brand.Status,
                //};
                
                return Ok(brand.Adapt<BrandResponse>());
            }

            return NotFound();
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] BrandRequest brandRequest)
        {
            var brand = await _brandRepository.CreateAsync(brandRequest.Adapt<Brand>());
            await _brandRepository.CommitAsync();

            if(brand is not null)
            {
                return Created($"{Request.Scheme}://{Request.Host}/api/Admin/Brands/{brand.Id}", brand.Adapt<BrandResponse>());
            }

            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] BrandRequest brandRequest)
        {
            var brand = _brandRepository.Update(brandRequest.Adapt<Brand>());
            await _brandRepository.CommitAsync();

            if (brand is not null)
            {
                return NoContent();
            }

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var brand = await _brandRepository.GetOneAsync(e => e.Id == id);

            if (brand is not null)
            {
                var result = _brandRepository.Delete(brand);
                await _brandRepository.CommitAsync();

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
