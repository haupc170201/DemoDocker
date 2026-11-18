using DemoVectorSearchWithOllama.Context;
using DemoVectorSearchWithOllama.Model;
using Microsoft.Data.SqlTypes;
using Microsoft.EntityFrameworkCore;


namespace DemoVectorSearchWithOllama.Services
{
    public record ProductInput(string Name, string Description);

    public record ProductSearchResult(int Id, string Name, string Description, double Distance);

    public class ProductService
    {
        private readonly ApplicationDbContext _context;
        private readonly EmbeddingService _embeddingService;

        public ProductService(
            ApplicationDbContext context,
            EmbeddingService embeddingService)
        {
            _context = context;
            _embeddingService = embeddingService;
        }

        public async Task<List<int>> CreateRangeAsync(
            IEnumerable<ProductInput> inputs)
        {
            // 1. Generate vectors using Ollama (combine name + description)
            var products = new List<Product>();

            foreach (var input in inputs)
            {
                var vector = await _embeddingService.GenerateAsync(
                    $"Name: {input.Name}\nDescription: {input.Description}");

                products.Add(new Product
                {
                    Name = input.Name,
                    Description = input.Description,
                    Embedding = new SqlVector<float>(vector)
                });
            }

            // 2. Save database in one batch
            _context.Products.AddRange(products);

            await _context.SaveChangesAsync();

            return products.Select(p => p.Id).ToList();
        }

        public async Task<List<ProductSearchResult>> SearchAsync(
            string query,
            int top = 5)
        {
            var vector = await _embeddingService.GenerateAsync(query);
            var queryVector = new SqlVector<float>(vector);

            double maxDistance = 0.4; // Ngưỡng lọc (bạn có thể tinh chỉnh từ 0.35 - 0.55)

            return await _context.Products
                .Where(p => EF.Functions.VectorDistance("cosine", p.Embedding, queryVector) <= maxDistance)
                .OrderBy(p => EF.Functions.VectorDistance("cosine", p.Embedding, queryVector))
                .Take(top)
                .Select(p => new ProductSearchResult(
                    p.Id,
                    p.Name,
                    p.Description,
                    EF.Functions.VectorDistance("cosine", p.Embedding, queryVector)))
                .ToListAsync();
        }
    }
}
