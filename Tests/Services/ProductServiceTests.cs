using Application.Features.Product.DTOs;
using Application.Features.ProductFeature.DTOs;
using Application.Features.ProductFeature.Service;
using AutoMapper;
using Domain.Entities;
using FluentAssertions;
using InfraStructure.Persistence.UnitOfWork;
using InfraStructure.Repositories.Generic;
using Moq;
using Tests.Fixtures.Fakes;
using Xunit;
namespace Tests.Services;

public class ProductServiceTests
{
    private readonly Mock<IMainInterFace<Product>> _productRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IUnitOfWork> _uow;

    public ProductServiceTests()
    {
        _mapperMock = new Mock<IMapper>();
        _productRepositoryMock = new Mock<IMainInterFace<Product>>();
        _uow = new Mock<IUnitOfWork>();

    }
    [Fact]
    public async Task GetProductById_ShouldReturnProduct_WhenProductExists()
    {
        var product = new Product
        {
            Id = 2,
            Price = 10.5m,
            Stock = 100
        };
        var productDto = new ProductDto
        {
            Id = product.Id,
            Price = product.Price,
            Stock = product.Stock
        };
        _mapperMock
            .Setup(x => x.Map<ProductDto>(It.IsAny<Product>()))
            .Returns(productDto);
        _productRepositoryMock.Setup(repo => repo.GetByID(product.Id)).ReturnsAsync(product);
        var service = new ProductService(_mapperMock.Object, _productRepositoryMock.Object, uow: _uow.Object);
        var result = await service.GetProductById(productDto.Id);
        result.Should().NotBeNull();
    }
    [Fact]
    public async Task GetProductById_ShouldReturnNull_WhenProductDoesnotExist()
    {
        var productDTO = new ProductDto
        {
            Id = 1
        };
        var product = new Product();
        _mapperMock.Setup(x => x.Map<Product>(productDTO)).Returns(product);
        _productRepositoryMock.Setup(x => x.GetByID(product.Id)).ReturnsAsync(product);
        var service = new ProductService(_mapperMock.Object, _productRepositoryMock.Object, _uow.Object);
        var result = await service.GetProductById(productDTO.Id);
        result.Should().BeNull();
    }
    [Fact]
    public async Task CreateProduct_ShouldCreateIt()
    {
        var product = new Product();
        var productdto = new CreateProductDto
        {
            CategoryId = 1,
            Description = "Unit Testing",
            Name = "Unit Test Product",
            Price = 1122,
            Stock = 0
        };
        _mapperMock.Setup(m => m.Map<Product>(productdto)).Returns(product);
        _productRepositoryMock.Setup(r => r.Create(product));
        var service = new ProductService(_mapperMock.Object, _productRepositoryMock.Object, _uow.Object);
        await service.Create(productdto);
        _productRepositoryMock.Verify(r => r.Create(product), Times.Once());
    }
    [Fact]
    public async Task DeleteProduct_ShouldThrowException()
    {
        var product = new Product();
        _productRepositoryMock.Setup(r => r.GetByID(1)).ReturnsAsync(product);
        var service = new ProductService(_mapperMock.Object, _productRepositoryMock.Object, _uow.Object);
        product.Id.Should().Be(0);
        await service.Delete(1);
        _productRepositoryMock.Verify(x => x.GetByID(1), Times.Once());
        _productRepositoryMock.Verify(x => x.Delete(product), Times.Once());
        _uow.Verify(u => u.SaveChangesAsync(), Times.Once());
    }
    [Fact]
    public async Task DeleteProduct_ShouldDeleteProduct()
    {
        var product = new Product();
        var Productdto = new ProductDto
        {
            Id = 1,
            Name = "Omar"
        };
        _mapperMock.Setup(x => x.Map<Product>(Productdto)).Returns(product);
        _productRepositoryMock.Setup(x=> x.GetByID(Productdto.Id)).ReturnsAsync(product);
        _productRepositoryMock.Setup(x => x.Delete(product));
        var service = new ProductService(_mapperMock.Object, _productRepositoryMock.Object, _uow.Object);
        var result = await service.Delete(Productdto.Id);
        product.Should().NotBeNull();
        result.Status.Should().BeTrue();
        result.msg.Should().BeNullOrEmpty();
    }
    [Fact]
    public async Task CreateFakeProduct()
    {
        var product = new Product
        {
            Id = 1,
            Price = 90
        };
        var repo = new FakeProductRepository();
        await repo.Create(product);
        repo.Products.Should().HaveCount(1);
        repo.Products.Should().Contain(product);
    }
    [Fact]
    public async Task GetFakeProductByID()
    {
        var product = new Product
        {
            Id = 1,
            Price = 90
        };
        var repo = new FakeProductRepository();
        await repo.Create(product);
        var fakeproduct = await repo.GetByID(product.Id);
        product.Should().BeSameAs(fakeproduct);
    }
    [Fact]
    public async Task GetTotalPrice()
    {
        var product = new Product
        {
            Id = 1,
            Price = 90,
            Stock = 1
        };
        var repo = new FakeProductRepository();
        await repo.Create(product);
        var totalprice = await repo.GetTotalAmount(product.Id);
        totalprice.Should().Be(product.Stock * product.Price);
    }
}
