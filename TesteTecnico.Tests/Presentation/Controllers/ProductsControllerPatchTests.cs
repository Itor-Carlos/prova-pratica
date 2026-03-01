using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TesteTecnico.Application.DTOs;
using TesteTecnico.Application.Interfaces;
using TesteTecnico.Domain.Enums;
using TesteTecnico.Presentation.Controllers;

namespace TesteTecnico.Tests.Presentation.Controllers;

public class ProductsControllerPatchTests
{
    [Fact]
    public async Task Patch_Should_ReturnOk_When_ProductExists()
    {
        var expectedId = Guid.NewGuid();
        var expected = new ProductResponse
        {
            Id = expectedId,
            Name = "Produto Atualizado",
            Description = "Descrição",
            Category = "Categoria",
            Price = 99.90m,
            Status = ProductStatus.Active,
            CreatedAt = DateTime.UtcNow.AddDays(-1),
            UpdatedAt = DateTime.UtcNow
        };

        var service = new FakeProductService
        {
            PatchResult = expected
        };

        var controller = new ProductsController(service);
        var request = new PatchProductRequest { Name = "Produto Atualizado", Price = 99.90m };

        var result = await controller.Patch(expectedId, request, CancellationToken.None);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var payload = Assert.IsType<ProductResponse>(okResult.Value);
        Assert.Equal(expectedId, payload.Id);
        Assert.Equal(expected.Name, payload.Name);
        Assert.Equal(expected.Price, payload.Price);
        Assert.Equal(expectedId, service.LastPatchedId);
        Assert.Same(request, service.LastPatchRequest);
    }

    [Fact]
    public async Task Patch_Should_ReturnNotFound_When_ProductDoesNotExist()
    {
        var service = new FakeProductService
        {
            PatchResult = null
        };

        var controller = new ProductsController(service);
        var request = new PatchProductRequest { Name = "Inexistente" };

        var result = await controller.Patch(Guid.NewGuid(), request, CancellationToken.None);

        Assert.IsType<NotFoundResult>(result);
    }

    private sealed class FakeProductService : IProductService
    {
        public ProductResponse? PatchResult { get; set; }
        public Guid LastPatchedId { get; private set; }
        public PatchProductRequest? LastPatchRequest { get; private set; }

        public Task<ProductResponse> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default)
            => Task.FromResult(new ProductResponse());

        public Task<ProductResponse?> UpdateAsync(Guid id, UpdateProductRequest request, CancellationToken cancellationToken = default)
            => Task.FromResult<ProductResponse?>(null);

        public Task<ProductResponse?> PatchAsync(Guid id, PatchProductRequest request, CancellationToken cancellationToken = default)
        {
            LastPatchedId = id;
            LastPatchRequest = request;
            return Task.FromResult(PatchResult);
        }

        public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
            => Task.FromResult(false);

        public Task<IReadOnlyList<ProductResponse>> GetAsync(ProductFilterRequest filter, CancellationToken cancellationToken = default)
            => Task.FromResult<IReadOnlyList<ProductResponse>>(Array.Empty<ProductResponse>());

        public Task<ProductResponse?> UploadImageAsync(Guid id, IFormFile file, CancellationToken cancellationToken = default)
            => Task.FromResult<ProductResponse?>(null);
    }
}
