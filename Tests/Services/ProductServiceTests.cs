using Application.Features.ProductFeature.Service;
using AutoMapper;
using Domain.Entities;
using FluentAssertions;
using InfraStructure.Repositories.Generic;
using Moq;
using Xunit;
namespace Tests.Services;

public class ProductServiceTests
{
    private readonly Mock<IMainInterFace<Product>> _productRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    [Fact]   
    public async Task GetProductById_ShouldReturnProduct()
    {
        var product = new Product
        {
            Id = 1,
            CategoryId = 1,
            Price = 10.5m,
            Stock = 100
        };
        _productRepositoryMock.Setup(repo => repo.GetByID(1)).ReturnsAsync(product);
        var service = new ProductService(_mapperMock.Object, _productRepositoryMock.Object);
        var result = await service.GetProductById(1);
        result.Should().NotBeNull();
    }

}
