using Microsoft.Extensions.AI;

public class EmbeddingService
{
    private readonly IEmbeddingGenerator<string, Embedding<float>> _embeddingGenerator;

    public EmbeddingService(
        IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator)
    {
        _embeddingGenerator = embeddingGenerator;
    }

    public async Task<float[]> GenerateAsync(string text)
    {
        var embedding = await _embeddingGenerator.GenerateAsync(text);

        return embedding.Vector.ToArray();
    }
}