namespace ProductsRecommendations.DTOs.Products;

public record CreateProductDto(
    string Title,
    string Category,
    string Summary,
    string Description);