using DemoVectorSearchWithOllama.Context;
using DemoVectorSearchWithOllama.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var endpoint = builder.Configuration["Ollama:Endpoint"]!;
var model = builder.Configuration["Ollama:EmbeddingModel"]!;

builder.Services.AddOllamaEmbeddingGenerator(
    modelId: model,
    endpoint: new Uri(endpoint)
);

builder.Services.AddScoped<EmbeddingService>();
builder.Services.AddScoped<ProductService>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
