using Microsoft.AspNetCore.Mvc;
using TesteTecnico.Application.DTOs;
using TesteTecnico.Application.Interfaces;
using TesteTecnico.Domain.Enums;

namespace TesteTecnico.Presentation.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    /// <summary>
    /// Cadastra um novo produto.
    /// </summary>
    /// <param name="request">Dados do produto a ser criado.</param>
    /// <param name="cancellationToken">Token para cancelamento da operação.</param>
    /// <returns>Produto criado.</returns>
    /// <response code="201">Produto criado com sucesso.</response>
    /// <response code="400">Requisição inválida.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateProductRequest request, CancellationToken cancellationToken)
    {
        var product = await _productService.CreateAsync(request, cancellationToken);
        return Created($"/api/products/{product.Id}", product);
    }

    /// <summary>
    /// Atualiza um produto existente.
    /// </summary>
    /// <param name="id">Identificador do produto.</param>
    /// <param name="request">Novos dados do produto.</param>
    /// <param name="cancellationToken">Token para cancelamento da operação.</param>
    /// <returns>Produto atualizado.</returns>
    /// <response code="200">Produto atualizado com sucesso.</response>
    /// <response code="404">Produto não encontrado.</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductRequest request, CancellationToken cancellationToken)
    {
        var product = await _productService.UpdateAsync(id, request, cancellationToken);
        return product is null ? NotFound() : Ok(product);
    }

    /// <summary>
    /// Remove um produto.
    /// </summary>
    /// <param name="id">Identificador do produto.</param>
    /// <param name="cancellationToken">Token para cancelamento da operação.</param>
    /// <returns>Sem conteúdo quando removido.</returns>
    /// <response code="204">Produto removido com sucesso.</response>
    /// <response code="404">Produto não encontrado.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await _productService.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }

    /// <summary>
    /// Lista produtos com filtros opcionais.
    /// </summary>
    /// <param name="category">Filtro por categoria.</param>
    /// <param name="minPrice">Preço mínimo para filtro.</param>
    /// <param name="maxPrice">Preço máximo para filtro.</param>
    /// <param name="status">Status do produto (Active/Inactive).</param>
    /// <param name="cancellationToken">Token para cancelamento da operação.</param>
    /// <returns>Lista de produtos filtrada.</returns>
    /// <response code="200">Consulta realizada com sucesso.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ProductResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(
        [FromQuery] string? category,
        [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice,
        [FromQuery] ProductStatus? status,
        CancellationToken cancellationToken)
    {
        var filter = new ProductFilterRequest
        {
            Category = category,
            MinPrice = minPrice,
            MaxPrice = maxPrice,
            Status = status
        };

        var products = await _productService.GetAsync(filter, cancellationToken);
        return Ok(products);
    }

    /// <summary>
    /// Realiza upload da imagem de um produto.
    /// </summary>
    /// <param name="id">Identificador do produto.</param>
    /// <param name="file">Arquivo de imagem.</param>
    /// <param name="cancellationToken">Token para cancelamento da operação.</param>
    /// <returns>Produto com URL da imagem atualizada.</returns>
    /// <response code="200">Imagem enviada com sucesso.</response>
    /// <response code="404">Produto não encontrado.</response>
    [HttpPost("{id:guid}/image")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UploadImage(Guid id, IFormFile file, CancellationToken cancellationToken)
    {
        var product = await _productService.UploadImageAsync(id, file, cancellationToken);
        return product is null ? NotFound() : Ok(product);
    }
}
