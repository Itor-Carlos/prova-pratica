using TesteTecnico.Domain.Enums;

namespace TesteTecnico.Application.DTOs;

public class ProductFilterRequest
{
    public string? Category { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public ProductStatus? Status { get; set; }
}
