
using Domain.Entities;

namespace Tests.Fixtures.Fakes;

public class FakeProductRepository
{
    public List<Product> Products = new List<Product>();
    public Task Create(Product product)
    {
        Products.Add(product);
        return Task.CompletedTask;
    }
    public Task<Product?> GetByID(int id)
    {
        return Task.FromResult(Products.FirstOrDefault(x => x.Id == id));
    }
    public Task<decimal> GetTotalAmount(int id)
    {
        var product = Products.FirstOrDefault(x=> x.Id == id);
        if (product == null)
            return Task.FromResult(0m);
        return Task.FromResult(product.Stock * product.Price);
    }
}
