using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using System.Reflection;
using TesteTecnico.Application.DTOs;
using TesteTecnico.Application.Interfaces;
using TesteTecnico.Application.Services;
using TesteTecnico.Domain.Repositories;
using TesteTecnico.Infrastructure.Persistence;
using TesteTecnico.Infrastructure.Persistence.Repositories;
using TesteTecnico.Infrastructure.Storage;
using TesteTecnico.Presentation.Middlewares;

var builder = WebApplication.CreateBuilder(args);

var webRootPath = Path.Combine(builder.Environment.ContentRootPath, "wwwroot");
Directory.CreateDirectory(webRootPath);
builder.WebHost.UseWebRoot(webRootPath);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "TesteTecnico API",
        Version = "v1",
        Description = "API REST para gerenciamento de produtos com cadastro, edição, exclusão, consulta com filtros e upload de imagem."
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }

    options.MapType<PatchProductRequest>(() => new OpenApiSchema
    {
        Type = "object",
        AdditionalPropertiesAllowed = false,
        Example = new OpenApiObject
        {
            ["name"] = new OpenApiString("Novo nome"),
            ["price"] = new OpenApiDouble(99.90)
        },
        Properties = new Dictionary<string, OpenApiSchema>
        {
            ["name"] = new() { Type = "string", MaxLength = 120 },
            ["description"] = new() { Type = "string", MaxLength = 1000 },
            ["category"] = new() { Type = "string", MaxLength = 80 },
            ["price"] = new() { Type = "number", Format = "decimal" },
            ["status"] = new() { Type = "integer", Format = "int32" }
        }
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IStorageService, LocalStorageService>();
builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseStaticFiles();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();