using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemoVectorSearchWithOllama.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmbeddingController : ControllerBase
    {
        private readonly EmbeddingService _service;

        public EmbeddingController(EmbeddingService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Generate(string text)
        {
            var vector = await _service.GenerateAsync(text);

            return Ok(new
            {
                Dimension = vector.Length,
                Vector = vector
            });
        }
    }
}
