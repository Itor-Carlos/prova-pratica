using TesteTecnico.Application.DTOs;
using TesteTecnico.Application.Interfaces;
using TesteTecnico.Domain.Entities;
using TesteTecnico.Domain.Exceptions;
using TesteTecnico.Domain.Repositories;

namespace TesteTecnico.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    private readonly IStorageService _storageService;

    public ProductService(IProductRepository repository, IStorageService storageService)
    {
        _repository = repository;
        _storageService = storageService;
    }

    public async Task<ProductResponse> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default)
    {
        var product = Product.Create(request.Name, request.Description, request.Category, request.Price, request.Status);

        await _repository.AddAsync(product, cancellationToken);

        return ProductResponse.FromEntity(product);
    }

    public async Task<ProductResponse?> UpdateAsync(Guid id, UpdateProductRequest request, CancellationToken cancellationToken = default)
    {
        var product = await _repository.GetByIdAsync(id, cancellationToken);
        if (product is null)
        {
            return null;
        }

        product.Update(request.Name, request.Description, request.Category, request.Price, request.Status);
        await _repository.UpdateAsync(product, cancellationToken);

        return ProductResponse.FromEntity(product);
    }

    public async Task<ProductResponse?> PatchAsync(Guid id, PatchProductRequest request, CancellationToken cancellationToken = default)
    {
        var product = await _repository.GetByIdAsync(id, cancellationToken);
        if (product is null)
        {
            return null;
        }

        var hasAnyField = request.Name is not null
            || request.Description is not null
            || request.Category is not null
            || request.Price.HasValue
            || request.Status.HasValue;

        if (!hasAnyField)
        {
            return ProductResponse.FromEntity(product);
        }

        var name = request.Name ?? product.Name;
        var description = request.Description ?? product.Description;
        var category = request.Category ?? product.Category;
        var price = request.Price ?? product.Price;
        var status = request.Status ?? product.Status;

        product.Update(name, description, category, price, status);
        await _repository.UpdateAsync(product, cancellationToken);

        return ProductResponse.FromEntity(product);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await _repository.GetByIdAsync(id, cancellationToken);
        if (product is null)
        {
            return false;
        }

        await _repository.DeleteAsync(product, cancellationToken);
        return true;
    }

    public async Task<IReadOnlyList<ProductResponse>> GetAsync(ProductFilterRequest filter, CancellationToken cancellationToken = default)
    {
        if (filter.MinPrice.HasValue && filter.MaxPrice.HasValue && filter.MinPrice.Value > filter.MaxPrice.Value)
        {
            throw new DomainException("Faixa de preço inválida: MinPrice não pode ser maior que MaxPrice.");
        }

        var products = await _repository.GetAsync(
            filter.Category,
            filter.MinPrice,
            filter.MaxPrice,
            filter.Status,
            cancellationToken);

        return products.Select(ProductResponse.FromEntity).ToList();
    }

    public async Task<ProductResponse?> UploadImageAsync(Guid id, IFormFile file, CancellationToken cancellationToken = default)
    {
        var product = await _repository.GetByIdAsync(id, cancellationToken);
        if (product is null)
        {
            return null;
        }

        var imageUrl = await _storageService.UploadAsync(file, cancellationToken);
        product.SetImage(imageUrl);

        await _repository.UpdateAsync(product, cancellationToken);

        return ProductResponse.FromEntity(product);
    }
}
