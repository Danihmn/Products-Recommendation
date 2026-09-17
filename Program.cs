using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel.Embeddings;
using OllamaSharp;
using Pgvector;
using ProductsRecommendations.Data;
using ProductsRecommendations.DTOs.Products;
using ProductsRecommendations.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(x => { x.UseNpgsql(connectionString, p => p.UseVector()); });

builder.Services.AddTransient<OllamaApiClient>(x =>
    new OllamaApiClient(uriString: "http://localhost:11434", defaultModel: "mxbai-embed-large"));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("v1/seed", async (AppDbContext context, OllamaApiClient ollama) =>
{
    var products = await context.Products.AsNoTracking().ToListAsync();
    var service = ollama.AsTextEmbeddingGenerationService();

    foreach (var product in products)
    {
        var embeddings = await service.GenerateEmbeddingAsync(product.Category);
        var recommendation = new Recommendation
        {
            Title = product.Title,
            Category = product.Category,
            Embedding = new Vector(embeddings)
        };

        context.Add(recommendation);
        await context.SaveChangesAsync();
    }

    return Results.Ok(new
    {
        message = "Ok"
    });
});

app.MapPost("v1/products",
    async (CreateProductDto dto, AppDbContext context, OllamaApiClient ollama) =>
    {
        var product = new Product
        {
            Title = dto.Title,
            Category = dto.Category,
            Summary = dto.Summary,
            Description = dto.Description,
        };

        context.Add(product);

        var service = ollama.AsTextEmbeddingGenerationService();
        var embedding = await service.GenerateEmbeddingAsync(dto.Category);

        var recommendation = new Recommendation
        {
            Title = product.Title,
            Category = product.Category,
            Embedding = new Vector(embedding),
        };

        context.Add(recommendation);

        await context.SaveChangesAsync();
        return Results.Created();
    });

app.MapPost("v1/prompt", async (QuestionDto dto, AppDbContext context, OllamaApiClient ollama) =>
{
    var service = ollama.AsTextEmbeddingGenerationService();
    var embeddings = await service.GenerateEmbeddingAsync(dto.Prompt);
});

app.Run();