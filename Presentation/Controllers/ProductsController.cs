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

    [HttpPost]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateProductRequest request, CancellationToken cancellationToken)
    {
        var product = await _productService.CreateAsync(request, cancellationToken);
        return Created($"/api/products/{product.Id}", product);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductRequest request, CancellationToken cancellationToken)
    {
        var product = await _productService.UpdateAsync(id, request, cancellationToken);
        return product is null ? NotFound() : Ok(product);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await _productService.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }

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

    [HttpPost("{id:guid}/image")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UploadImage(Guid id, [FromForm] IFormFile file, CancellationToken cancellationToken)
    {
        var product = await _productService.UploadImageAsync(id, file, cancellationToken);
        return product is null ? NotFound() : Ok(product);
    }
}
