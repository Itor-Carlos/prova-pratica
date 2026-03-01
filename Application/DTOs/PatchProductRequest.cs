using System.ComponentModel.DataAnnotations;
using TesteTecnico.Domain.Enums;

namespace TesteTecnico.Application.DTOs;

/// <summary>
/// Campos opcionais para atualização parcial de produto.
/// Envie somente as propriedades que deseja alterar.
/// </summary>
public class PatchProductRequest
{
    /// <summary>
    /// Nome do produto (máximo de 120 caracteres).
    /// </summary>
    [MaxLength(120)]
    public string? Name { get; set; }

    /// <summary>
    /// Descrição do produto (máximo de 1000 caracteres).
    /// </summary>
    [MaxLength(1000)]
    public string? Description { get; set; }

    /// <summary>
    /// Categoria do produto (máximo de 80 caracteres).
    /// </summary>
    [MaxLength(80)]
    public string? Category { get; set; }

    /// <summary>
    /// Preço do produto. Deve ser maior ou igual a 0.01.
    /// </summary>
    [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
    public decimal? Price { get; set; }

    /// <summary>
    /// Status do produto: 1 = Active, 2 = Inactive.
    /// </summary>
    public ProductStatus? Status { get; set; }
}