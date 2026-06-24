using Application.Features.Product.DTOs;
using Application.Features.ProductFeature.DTOs;
using Application.Features.ProductFeature.Service;
using AutoMapper;
using Domain.Entities;
using FluentAssertions;
using InfraStructure.Persistence;
using InfraStructure.Persistence.UnitOfWork;
using InfraStructure.Repositories.Generic;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Moq;
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
    public async Task GetProductById_ShouldReturnProduct()
    {
        var product = new Product
        {
            Id = 2,
            Price = 10.5m,
            Stock = 100
        };
        var productDto = new ProductDTO
        {
            Id = product.Id,
            Price = product.Price,
            Stock = product.Stock
        };
        _mapperMock
            .Setup(x => x.Map<ProductDTO>(It.IsAny<Product>()))
            .Returns(productDto);
        _productRepositoryMock.Setup(repo => repo.GetByID(product.Id)).ReturnsAsync(product);
        var service = new ProductService(_mapperMock.Object, _productRepositoryMock.Object, uow: _uow.Object);
        var result = await service.GetProductById(productDto.Id);
        result.Should().NotBeNull();
    }
    [Fact]
    public async Task GetProductById_ShouldReturnNull()
    {
        var productDTO = new ProductDTO
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
        var productdto = new CreateProductDTO
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
        _productRepositoryMock.Verify(x=> x.GetByID(1), Times.Once());
        _productRepositoryMock.Verify(x=>x.Delete(product) , Times.Once());
        _uow.Verify(u=> u.SaveChangesAsync(), Times.Once());
    }

}
