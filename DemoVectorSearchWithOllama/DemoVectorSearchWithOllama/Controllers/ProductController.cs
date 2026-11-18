using DemoVectorSearchWithOllama.Services;
using Microsoft.AspNetCore.Mvc;

namespace DemoVectorSearchWithOllama.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _service;

        public ProductController(ProductService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] List<CreateProductRequest> requests)
        {
            if (requests == null || requests.Count == 0)
            {
                return BadRequest("At least one product is required.");
            }

            if (requests.Any(r => string.IsNullOrWhiteSpace(r.Name) || string.IsNullOrWhiteSpace(r.Description)))
            {
                return BadRequest("Name and Description are required for every product.");
            }

            var ids = await _service.CreateRangeAsync(
                requests.Select(r => new ProductInput(r.Name, r.Description)));

            return Ok(new { Ids = ids });
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string query, [FromQuery] int top = 5)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Query is required.");
            }

            var results = await _service.SearchAsync(query, top);

            return Ok(results);
        }

        public sealed class CreateProductRequest
        {
            public string Name { get; set; } = string.Empty;

            public string Description { get; set; } = string.Empty;
        }
    }
}
