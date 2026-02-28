using TesteTecnico.Domain.Enums;
using TesteTecnico.Domain.Exceptions;

namespace TesteTecnico.Domain.Entities;

public class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string Category { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public ProductStatus Status { get; private set; }
    public string? ImageUrl { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private Product()
    {
    }

    public static Product Create(string name, string description, string category, decimal price, ProductStatus status)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        product.Update(name, description, category, price, status);
        return product;
    }

    public void Update(string name, string description, string category, decimal price, ProductStatus status)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Nome do produto é obrigatório.");

        if (string.IsNullOrWhiteSpace(category))
            throw new DomainException("Categoria é obrigatória.");

        if (price <= 0)
            throw new DomainException("Preço deve ser maior que zero.");

        Name = name.Trim();
        Description = description?.Trim() ?? string.Empty;
        Category = category.Trim();
        Price = price;
        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetImage(string imageUrl)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
            throw new DomainException("URL da imagem é obrigatória.");

        ImageUrl = imageUrl;
        UpdatedAt = DateTime.UtcNow;
    }
}
