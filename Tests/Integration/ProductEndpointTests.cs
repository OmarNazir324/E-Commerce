
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

public class ProductEndpointTests :IClassFixture<IntegrationTestFixture>
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
        HttpClient _client = _factory.CreateClient();
        DatabaseHelper.ResetDB(_factory);
        _factory.Database_Name = DatabaseHelper.GetDBName("GetProducts_");
        await LoginHelperForIntegrationTest.AuthenticateAsync(_client, $"Login_{Guid.NewGuid()}@gmail.com","Test123@");
        var response = await _client.GetAsync("/api/Product");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
   
    [Fact]
    public async Task CreateProduct_ShouldReturn200OKAndPersistToEnd()
    {
        HttpClient _client = _factory.CreateClient();
        DatabaseHelper.ResetDB(_factory);
        _factory.Database_Name = DatabaseHelper.GetDBName("CreateProduct_");
        await LoginHelperForIntegrationTest.AuthenticateAsync(_client, $"Login_{Guid.NewGuid()}@gmail.com", "Test123@");
        var response = await ProductFactory.CreateProductAsync(_client,stock: 100);
        var responsebody = await response.Content.ReadFromJsonAsync<ApiResponse<Product>>();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responsebody.Should().NotBeNull();
        responsebody.Data.Should().NotBeNull();
        responsebody.TotalRecords.Should().Be(1);
    }
    [Fact]
    public async Task GetProducts_ShouldCountOneAfterCreating()
    {
        HttpClient _client = _factory.CreateClient();
        DatabaseHelper.ResetDB(_factory);
        _factory.Database_Name = DatabaseHelper.GetDBName("GetProducts_");
        await LoginHelperForIntegrationTest.AuthenticateAsync(_client, $"Login_{Guid.NewGuid()}@gmail.com", "Test123@");
        await ProductFactory.CreateProductAsync(_client,name: "Test Product Factory");
        var response = await _client.GetAsync("/api/Product");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responsebody = await response.Content.ReadFromJsonAsync<IEnumerable<ProductDTO>>();
        responsebody.Should().ContainSingle();
    }
    
    [Fact]
    public async Task GetProductByID_ShouldReturnProduct()
    {
        HttpClient _client = _factory.CreateClient();
        DatabaseHelper.ResetDB(_factory);
        _factory.Database_Name = DatabaseHelper.GetDBName("GetProduct_");
        await LoginHelperForIntegrationTest.AuthenticateAsync(_client, $"Login_{Guid.NewGuid()}@gmail.com", "Test123@");
        var createresponse = await ProductFactory.CreateProductAsync(_client);
        var createresponseboby = await createresponse.Content.ReadFromJsonAsync<ApiResponse<Product>>();
        var response = await _client.GetAsync("/api/Product/" + createresponseboby.Data.Id);
        var responsebody = await response.Content.ReadFromJsonAsync<ProductDTO>();
        responsebody.Should().BeOfType(typeof(ProductDTO));
        responsebody.Description.Should().Be("IntegrationTest");
    }
    [Fact]
    public async Task GetProductByID_ShouldReturn401UnAuthorized()
    {
        HttpClient _client = _factory.CreateClient();
        DatabaseHelper.ResetDB(_factory);
        _factory.Database_Name = DatabaseHelper.GetDBName("GetProduct_");
        var response = await _client.GetAsync("/api/Product/1");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    [Fact]
    public async Task GetProductByID_ShouldReturn404NotFound()
    {
        HttpClient _client = _factory.CreateClient();
        DatabaseHelper.ResetDB(_factory);
        _factory.Database_Name = DatabaseHelper.GetDBName("GetProduct_");
        await LoginHelperForIntegrationTest.AuthenticateAsync(_client, $"Login_{Guid.NewGuid()}@gmail.com", "Test123@");
        var response = await _client.GetAsync("/api/Product/1");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    [Fact]
    public async Task UpdateProduct_ShoukdReturn200OK()
    {
        HttpClient _client = _factory.CreateClient();
        DatabaseHelper.ResetDB(_factory);
        _factory.Database_Name = DatabaseHelper.GetDBName("UpdateProduct_");
        await LoginHelperForIntegrationTest.AuthenticateAsync(_client, $"Login_{Guid.NewGuid()}@gmail.com", "Test123@");
        var createresponse = await ProductFactory.CreateProductAsync(_client,price: 200,cat_id:9,name: "Updated Product");
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
        var getResponse =
    await _client.GetAsync(
        $"/api/Product/{updatedto.Id}"
    );

        var product =
            await getResponse.Content
                .ReadFromJsonAsync<ProductDTO>();

        product.Price.Should().Be(200);
        product.Name.Should().Be("Updated Product");
    }
    [Fact] 
    public async Task DeleteProduct_ShouldReturn200AndDeleteProduct()
    {
        HttpClient _client = _factory.CreateClient();
        DatabaseHelper.ResetDB(_factory);
        _factory.Database_Name = DatabaseHelper.GetDBName("DeleteProduct_");
        await LoginHelperForIntegrationTest.AuthenticateAsync(_client, $"Login_{Guid.NewGuid()}@gmail.com", "Test123@");
        var createresponse = await ProductFactory.CreateProductAsync(_client);
        var createresposebody = await createresponse.Content.ReadFromJsonAsync<ApiResponse<Product>>();
        var response = await _client.DeleteAsync("/api/Product/" + createresposebody.Data.Id);
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var productresponse = await _client.GetAsync("/api/Product/" + createresposebody.Data.Id);
        var product = await productresponse.Content.ReadFromJsonAsync<ProductDTO>();
        productresponse.StatusCode
        .Should()
        .Be(HttpStatusCode.NotFound);
       // product.Id.Should().Be(0);
        // product.Name.Should().BeNullOrEmpty();

    }
}
