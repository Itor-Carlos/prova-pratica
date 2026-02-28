using System.ComponentModel.DataAnnotations;
using TesteTecnico.Domain.Enums;

namespace TesteTecnico.Application.DTOs;

public class CreateProductRequest
{
    [Required]
    [MaxLength(120)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [MaxLength(80)]
    public string Category { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }

    public ProductStatus Status { get; set; } = ProductStatus.Active;
}
