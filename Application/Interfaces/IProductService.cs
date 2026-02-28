using TesteTecnico.Application.DTOs;

namespace TesteTecnico.Application.Interfaces;

public interface IProductService
{
    Task<ProductResponse> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default);
    Task<ProductResponse?> UpdateAsync(Guid id, UpdateProductRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ProductResponse>> GetAsync(ProductFilterRequest filter, CancellationToken cancellationToken = default);
    Task<ProductResponse?> UploadImageAsync(Guid id, IFormFile file, CancellationToken cancellationToken = default);
}
