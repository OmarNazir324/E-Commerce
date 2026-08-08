
using API;
using Application.Features.Product.DTOs;
using Application.Features.ProductFeature.DTOs;
using Application.Responses;
using Domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.Http.Json;
using Tests.Helpers;
using Xunit;

namespace Tests.Integration;

public class ProductEndpointTests :IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;
    public ProductEndpointTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }
    [Fact]
    // Done
    public async Task GetProducts_ShouldReturnSuccess()
    {
        DatabaseHelper.ResetDB(_factory);
        _factory.Database_Name = DatabaseHelper.GetDBName("GetProducts_");
        await LoginHelperForIntegrationTest.AuthenticateAsync(_client, $"Login_{Guid.NewGuid()}@gmail.com","Test123@");
        var response = await _client.GetAsync("/api/Product");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
   
    [Fact]
    public async Task CreateProduct_ShouldReturn200OKAndPersistToEnd()
    {
        DatabaseHelper.ResetDB(_factory);
        _factory.Database_Name = DatabaseHelper.GetDBName("CreateProduct_");
        await LoginHelperForIntegrationTest.AuthenticateAsync(_client, $"Login_{Guid.NewGuid()}@gmail.com", "Test123@");
        var response = await CreateProductAsync();
        var responsebody = await response.Content.ReadFromJsonAsync<ApiResponse<Product>>();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responsebody.Should().NotBeNull();
        responsebody.Data.Should().NotBeNull();
        responsebody.TotalRecords.Should().Be(1);
    }
    [Fact]
    public async Task GetProducts_ShouldCountOneAfterCreating()
    {
        DatabaseHelper.ResetDB(_factory);
        _factory.Database_Name = DatabaseHelper.GetDBName("GetProducts_");
        await LoginHelperForIntegrationTest.AuthenticateAsync(_client, $"Login_{Guid.NewGuid()}@gmail.com", "Test123@");
        await CreateProductAsync();
        var response = await _client.GetAsync("/api/Product");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responsebody = await response.Content.ReadFromJsonAsync<IEnumerable<ProductDTO>>();
        responsebody.Should().ContainSingle();
    }
    private async Task<HttpResponseMessage> CreateProductAsync()
    {
        var productdto = new CreateProductDTO
        {
            CategoryId = 1,
            Description = "IntegrationTest",
            Name = "Integration Test",
            Price = 90,
            Stock = 9
        };
        return await _client.PostAsJsonAsync("/api/Product", productdto);
    }
    [Fact]
    public async Task GetProductByID_ShouldReturnProduct()
    {
        DatabaseHelper.ResetDB(_factory);
        _factory.Database_Name = DatabaseHelper.GetDBName("GetProduct_");
        await LoginHelperForIntegrationTest.AuthenticateAsync(_client, $"Login_{Guid.NewGuid()}@gmail.com", "Test123@");
        var createresponse =  await CreateProductAsync();
        var createresponseboby = await createresponse.Content.ReadFromJsonAsync<ApiResponse<Product>>();
        var response = await _client.GetAsync("/api/Product/" + createresponseboby.Data.Id);
        var responsebody = await response.Content.ReadFromJsonAsync<ProductDTO>();
        responsebody.Should().BeOfType(typeof(ProductDTO));
        responsebody.Description.Should().Be("IntegrationTest");
    }
    [Fact]
    public async Task GetProductByID_ShouldReturn401UnAuthorized()
    {
        DatabaseHelper.ResetDB(_factory);
        _factory.Database_Name = DatabaseHelper.GetDBName("GetProduct_");
        var response = await _client.GetAsync("/api/Product/1");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    [Fact]
    public async Task GetProductByID_ShouldReturn404NotFound()
    {
        DatabaseHelper.ResetDB(_factory);
        _factory.Database_Name = DatabaseHelper.GetDBName("GetProduct_");
        await LoginHelperForIntegrationTest.AuthenticateAsync(_client, $"Login_{Guid.NewGuid()}@gmail.com", "Test123@");
        var response = await _client.GetAsync("/api/Product/1");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    [Fact]
    public async Task UpdateProduct_ShoukdReturn200OK()
    {
        DatabaseHelper.ResetDB(_factory);
        _factory.Database_Name = DatabaseHelper.GetDBName("UpdateProduct_");
        await LoginHelperForIntegrationTest.AuthenticateAsync(_client, $"Login_{Guid.NewGuid()}@gmail.com", "Test123@");
        var createresponse = await this.CreateProductAsync();
        var createresposebody = await createresponse.Content.ReadFromJsonAsync<ApiResponse<Product>>();
        var updatedto = new UpdateProductDTO
        {
            Id = createresposebody.Data.Id,
            CategoryId = createresposebody.Data.CategoryId,
            Description = createresposebody.Data.Description + " Edited To Test Update Product In Integration Test",
            Name = createresposebody.Data.Name,
            Price = createresposebody.Data.Price,
            Stock = createresposebody.Data.Stock
        };
        var response = await _client.PutAsJsonAsync("/api/Product", updatedto);
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    [Fact] 
    public async Task DeleteProduct_ShouldReturn200AndDeleteProduct()
    {
        DatabaseHelper.ResetDB(_factory);
        _factory.Database_Name = DatabaseHelper.GetDBName("DeleteProduct_");
        await LoginHelperForIntegrationTest.AuthenticateAsync(_client, $"Login_{Guid.NewGuid()}@gmail.com", "Test123@");
        var createresponse = await this.CreateProductAsync();
        var createresposebody = await createresponse.Content.ReadFromJsonAsync<ApiResponse<Product>>();
        var response = await _client.DeleteAsync("/api/Product/" + createresposebody.Data.Id);
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var productresponse = await _client.GetAsync("/api/Product/" + createresposebody.Data.Id);
        var product = await productresponse.Content.ReadFromJsonAsync<ProductDTO>();
        product.Id.Should().Be(0);
        product.Name.Should().BeNullOrEmpty();

    }
}
