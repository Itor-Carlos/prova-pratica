using TesteTecnico.Application.Interfaces;
using TesteTecnico.Domain.Exceptions;

namespace TesteTecnico.Infrastructure.Storage;

public class LocalStorageService : IStorageService
{
    private static readonly HashSet<string> AllowedExtensions =
    [
        ".jpg",
        ".jpeg",
        ".png",
        ".webp"
    ];

    private readonly IWebHostEnvironment _environment;

    public LocalStorageService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<string> UploadAsync(IFormFile file, CancellationToken cancellationToken = default)
    {
        if (file.Length == 0)
        {
            throw new DomainException("Arquivo de imagem está vazio.");
        }

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!AllowedExtensions.Contains(extension))
        {
            throw new DomainException("Formato de imagem não permitido. Use jpg, jpeg, png ou webp.");
        }

        var fileName = $"{Guid.NewGuid():N}{extension}";

        var rootPath = _environment.WebRootPath;
        if (string.IsNullOrWhiteSpace(rootPath))
        {
            rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        }

        var uploadFolder = Path.Combine(rootPath, "uploads");
        Directory.CreateDirectory(uploadFolder);

        var fullPath = Path.Combine(uploadFolder, fileName);

        await using var stream = new FileStream(fullPath, FileMode.Create);
        await file.CopyToAsync(stream, cancellationToken);

        return $"/uploads/{fileName}";
    }
}
