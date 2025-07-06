using Microsoft.AspNetCore.Mvc;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Threading.Tasks;

namespace ECommerce512.API.Areas.Admin.Controllers
{
    [Route("api/[area]/[controller]")]
    [Area("Admin")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productRepository.GetAsync();

            //var productResponses = products.Select(e => new ProductResponse()
            //{
            //    Id = e.Id,
            //    Description = e.Description,
            //    Name = e.Name,
            //    Status = e.Status,
            //});
            
            return Ok(products.Adapt<IEnumerable<ProductResponse>>());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne([FromRoute] int id)
        {
            var product = await _productRepository.GetOneAsync(e => e.Id == id);

            if (product is not null)
            {
                //var productResponse = new ProductResponse()
                //{
                //    Id = product.Id,
                //    Description = product.Description,
                //    Name = product.Name,
                //    Status = product.Status,
                //};
                
                return Ok(product.Adapt<ProductResponse>());
            }

            return NotFound();
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromForm] ProductRequest productRequest)
        {
            if (productRequest.MainImg != null && productRequest.MainImg.Length > 0)
            {
                // Add new img
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(productRequest.MainImg.FileName);

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Images", fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    await productRequest.MainImg.CopyToAsync(stream);
                }

                // Update img in Db
                var product = productRequest.Adapt<Product>();
                product.MainImg = fileName;

                var productCreated = await _productRepository.CreateAsync(product);
                await _productRepository.CommitAsync();

                if (productCreated is not null)
                {
                    return Created($"{Request.Scheme}://{Request.Host}/api/Admin/Products/{productCreated.Id}", productCreated.Adapt<ProductResponse>());
                }

                return BadRequest();
            }

            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromForm] ProductRequest productRequest)
        {
            var productInDb = await _productRepository.GetOneAsync(e => e.Id == id, tracked: false);

            if(productInDb is not null)
            {
                var product = productRequest.Adapt<Product>();

                if (productRequest.MainImg != null && productRequest.MainImg.Length > 0)
                {
                    // Add new img
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(productRequest.MainImg.FileName);

                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "images", fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await productRequest.MainImg.CopyToAsync(stream);
                    }

                    // Delete old img from wwwroot
                    var oldFileName = productInDb.MainImg;
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "images", oldFileName);

                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }

                    // Update img in Db
                    product.MainImg = fileName;
                }
                else
                {
                    // Save the old product img
                    product.MainImg = productInDb.MainImg;
                }

                var productCreated = _productRepository.Update(product);
                await _productRepository.CommitAsync();

                if (product is not null)
                {
                    return NoContent();
                }

                return BadRequest();
            }

            return NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var product = await _productRepository.GetOneAsync(e => e.Id == id);

            if (product is not null)
            {
                var oldFileName = product.MainImg;
                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "images", oldFileName);

                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }

                var result = _productRepository.Delete(product);
                await _productRepository.CommitAsync();

                if(result)
                {
                    return NoContent();
                }

                return BadRequest();
            }

            return NotFound();
        }

        [HttpDelete("DeleteImg/{id}")]
        public async Task<IActionResult> DeleteImg(int id)
        {
            var product = await _productRepository.GetOneAsync(e => e.Id == id);

            if (product is not null)
            {
                var oldFileName = product.MainImg;
                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "images", oldFileName);

                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }

                product.MainImg = "defaultImg.png";
                await _productRepository.CommitAsync();

                return NoContent();
            }

            return NotFound();
        }
    }
}
