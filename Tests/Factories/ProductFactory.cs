using Application.Features.ProductFeature.DTOs;
using System.Net.Http.Json;

namespace Tests.Factories;

public static class ProductFactory
{
    public static async Task<HttpResponseMessage> CreateProductAsync(HttpClient _client,int? cat_id =null, String? desc = null,String? name = null,int? price = null,int? stock = null)
    {
        var productdto = new CreateProductDTO
        {
            CategoryId = cat_id ?? 1,
            Description = desc ?? "IntegrationTest",
            Name = name ?? "Integration Test",
            Price = price ?? 90,
            Stock = stock ?? 9
        };
        return await _client.PostAsJsonAsync("/api/Product", productdto);
    }
}
