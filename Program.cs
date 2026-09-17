using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel.Embeddings;
using OllamaSharp;
using Pgvector;
using ProductsRecommendations.Data;
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

        context.Recommendations.Add(recommendation);
        await context.SaveChangesAsync();
    }

    return Results.Ok(new
    {
        message = "Ok"
    });
});

app.Run();