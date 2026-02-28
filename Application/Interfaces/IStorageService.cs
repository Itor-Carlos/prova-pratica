namespace TesteTecnico.Application.Interfaces;

public interface IStorageService
{
    Task<string> UploadAsync(IFormFile file, CancellationToken cancellationToken = default);
}
