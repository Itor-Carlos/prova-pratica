namespace TesteTecnico.Tests.Domain.Entities;

public class ProductTests
{
    [Fact]
    public void Create_Should_TrimTextFields_AndSetAuditFields()
    {
        var product = Product.Create("  Produto  ", "  Descrição  ", "  Categoria  ", 12.34m, ProductStatus.Active);

        Assert.Equal("Produto", product.Name);
        Assert.Equal("Descrição", product.Description);
        Assert.Equal("Categoria", product.Category);
        Assert.Equal(12.34m, product.Price);
        Assert.Equal(ProductStatus.Active, product.Status);
        Assert.NotEqual(Guid.Empty, product.Id);
        Assert.True(product.CreatedAt <= DateTime.UtcNow);
        Assert.True(product.UpdatedAt <= DateTime.UtcNow);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_Should_Throw_When_NameIsInvalid(string invalidName)
    {
        var exception = Assert.Throws<DomainException>(() =>
            Product.Create(invalidName, "Descrição", "Categoria", 10m, ProductStatus.Active));

        Assert.Equal("Nome do produto é obrigatório.", exception.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_Should_Throw_When_CategoryIsInvalid(string invalidCategory)
    {
        var exception = Assert.Throws<DomainException>(() =>
            Product.Create("Produto", "Descrição", invalidCategory, 10m, ProductStatus.Active));

        Assert.Equal("Categoria é obrigatória.", exception.Message);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public void Create_Should_Throw_When_PriceIsNotPositive(decimal invalidPrice)
    {
        var exception = Assert.Throws<DomainException>(() =>
            Product.Create("Produto", "Descrição", "Categoria", invalidPrice, ProductStatus.Active));

        Assert.Equal("Preço deve ser maior que zero.", exception.Message);
    }

    [Fact]
    public void Update_Should_TrimTextFields_AndRefreshUpdatedAt()
    {
        var product = Product.Create("Produto", "Descrição", "Categoria", 10m, ProductStatus.Active);
        var previousUpdatedAt = product.UpdatedAt;

        Thread.Sleep(10);
        product.Update("  Novo Nome  ", "  Nova Descrição  ", "  Nova Categoria  ", 20m, ProductStatus.Inactive);

        Assert.Equal("Novo Nome", product.Name);
        Assert.Equal("Nova Descrição", product.Description);
        Assert.Equal("Nova Categoria", product.Category);
        Assert.Equal(20m, product.Price);
        Assert.Equal(ProductStatus.Inactive, product.Status);
        Assert.True(product.UpdatedAt > previousUpdatedAt);
    }

    [Fact]
    public void Update_Should_SetEmptyString_When_DescriptionIsNull()
    {
        var product = Product.Create("Produto", "Descrição", "Categoria", 10m, ProductStatus.Active);

        product.Update("Produto", null!, "Categoria", 15m, ProductStatus.Active);

        Assert.Equal(string.Empty, product.Description);
    }
}
