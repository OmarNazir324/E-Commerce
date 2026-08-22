
using API;
using Application.Features.Product.DTOs;
using Application.Features.ProductFeature.DTOs;
using Application.Responses;
using Domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.Http.Json;
using Tests.Factories;
using Tests.Fixtures;
using Tests.Helpers;
using Xunit;

namespace Tests.Integration;

public class ProductEndpointTests : IClassFixture<IntegrationTestFixture>
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    public ProductEndpointTests(IntegrationTestFixture integrationTestFixture)
    {
        _factory = integrationTestFixture.Factory;
    }
    [Fact]
    // Done
    public async Task GetProducts_ShouldReturnSuccess()
    {
        #region Arrange
        HttpClient _client = _factory.CreateClient();
        DatabaseHelper.ResetDB(_factory);
        _factory.Database_Name = DatabaseHelper.GetDBName("GetProducts_");
        #endregion
        #region Act
        await LoginHelperForIntegrationTest.AuthenticateAsync(_client, $"Login_{Guid.NewGuid()}@gmail.com", "Test123@");
        var response = await _client.GetAsync("/api/Product");
        #endregion
        #region Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        #endregion
    }

    [Fact]
    public async Task CreateProduct_ShouldReturn200OKAndPersistToEnd()
    {
        #region Arrange
        HttpClient _client = _factory.CreateClient();
        DatabaseHelper.ResetDB(_factory);
        _factory.Database_Name = DatabaseHelper.GetDBName("CreateProduct_");
        #endregion
        #region Act
        await LoginHelperForIntegrationTest.AuthenticateAsync(_client, $"Login_{Guid.NewGuid()}@gmail.com", "Test123@");
        var response = await ProductFactory.CreateProductAsync(_client, stock: 100);
        #endregion
        #region Assert
        var responsebody = await response.Content.ReadFromJsonAsync<ApiResponse<Product>>();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responsebody.Should().NotBeNull();
        responsebody.Data.Should().NotBeNull();
        responsebody.TotalRecords.Should().Be(1);
        #endregion
    }
    [Fact]
    public async Task GetProducts_ShouldCountOneAfterCreating()
    {
        #region Arrange
        HttpClient _client = _factory.CreateClient();
        DatabaseHelper.ResetDB(_factory);
        _factory.Database_Name = DatabaseHelper.GetDBName("GetProducts_");
        #endregion
        #region Act
        await LoginHelperForIntegrationTest.AuthenticateAsync(_client, $"Login_{Guid.NewGuid()}@gmail.com", "Test123@");
        await ProductFactory.CreateProductAsync(_client, name: "Test Product Factory");
        var response = await _client.GetAsync("/api/Product");
        #endregion
        #region Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responsebody = await response.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>();
        responsebody.Should().ContainSingle();
        #endregion
    }

    [Fact]
    public async Task GetProductByID_ShouldReturnProduct()
    {
        #region Arrange
        HttpClient _client = _factory.CreateClient();
        DatabaseHelper.ResetDB(_factory);
        _factory.Database_Name = DatabaseHelper.GetDBName("GetProduct_");
        #endregion
        #region Act
        await LoginHelperForIntegrationTest.AuthenticateAsync(_client, $"Login_{Guid.NewGuid()}@gmail.com", "Test123@");
        var createresponse = await ProductFactory.CreateProductAsync(_client);
        var createresponseboby = await createresponse.Content.ReadFromJsonAsync<ApiResponse<Product>>();
        var response = await _client.GetAsync("/api/Product/" + createresponseboby.Data.Id);
        #endregion
        #region Assert
        var responsebody = await response.Content.ReadFromJsonAsync<ProductDto>();
        responsebody.Should().BeOfType(typeof(ProductDto));
        responsebody.Description.Should().Be("IntegrationTest");
        #endregion
    }
    [Fact]
    public async Task GetProductByID_ShouldReturn401UnAuthorized()
    {
        #region Arrange
        HttpClient _client = _factory.CreateClient();
        DatabaseHelper.ResetDB(_factory);
        _factory.Database_Name = DatabaseHelper.GetDBName("GetProduct_");
        #endregion
        #region Act
        var response = await _client.GetAsync("/api/Product/1");
        #endregion
        #region Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        #endregion
    }
    [Fact]
    public async Task GetProductByID_ShouldReturn404NotFound()
    {
        #region Arrange
        HttpClient _client = _factory.CreateClient();
        DatabaseHelper.ResetDB(_factory);
        _factory.Database_Name = DatabaseHelper.GetDBName("GetProduct_");
        #endregion
        #region Act
        await LoginHelperForIntegrationTest.AuthenticateAsync(_client, $"Login_{Guid.NewGuid()}@gmail.com", "Test123@");
        var response = await _client.GetAsync("/api/Product/1");
        #endregion
        #region Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        #endregion
    }
    [Fact]
    public async Task UpdateProduct_ShouldReturn200OK()
    {
        #region Arrange
        HttpClient _client = _factory.CreateClient();
        DatabaseHelper.ResetDB(_factory);
        _factory.Database_Name = DatabaseHelper.GetDBName("UpdateProduct_");
        #endregion
        #region Act
        await LoginHelperForIntegrationTest.AuthenticateAsync(_client, $"Login_{Guid.NewGuid()}@gmail.com", "Test123@");
        var createresponse = await ProductFactory.CreateProductAsync(_client, price: 200, cat_id: 9, name: "Updated Product");
        var createresposebody = await createresponse.Content.ReadFromJsonAsync<ApiResponse<Product>>();
        var updatedto = new UpdateProductDto
        {
            Id = createresposebody.Data.Id,
            CategoryId = createresposebody.Data.CategoryId,
            Description = createresposebody.Data.Description + " Edited To Test Update Product In Integration Test",
            Name = createresposebody.Data.Name,
            Price = createresposebody.Data.Price,
            Stock = createresposebody.Data.Stock
        };
        var response = await _client.PutAsJsonAsync("/api/Product", updatedto);
        #endregion
        #region Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var getResponse = await _client.GetAsync($"/api/Product/{updatedto.Id}");
        var product = await getResponse.Content
                .ReadFromJsonAsync<ProductDto>();
        product.Price.Should().Be(200);
        product.Name.Should().Be("Updated Product");
        #endregion
    }
    [Fact]
    public async Task DeleteProduct_ShouldReturn200AndDeleteProduct()
    {
        #region Arange
        HttpClient _client = _factory.CreateClient();
        DatabaseHelper.ResetDB(_factory);
        _factory.Database_Name = DatabaseHelper.GetDBName("DeleteProduct_");
        #endregion
        #region Act
        await LoginHelperForIntegrationTest.AuthenticateAsync(_client, $"Login_{Guid.NewGuid()}@gmail.com", "Test123@");
        var createresponse = await ProductFactory.CreateProductAsync(_client);
        var createresposebody = await createresponse.Content.ReadFromJsonAsync<ApiResponse<Product>>();
        var response = await _client.DeleteAsync("/api/Product/" + createresposebody.Data.Id);
        #endregion
        #region Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var productresponse = await _client.GetAsync("/api/Product/" + createresposebody.Data.Id);
        var product = await productresponse.Content.ReadFromJsonAsync<ProductDto>();
        productresponse.StatusCode
        .Should()
        .Be(HttpStatusCode.NotFound);
        #endregion
    }
}
