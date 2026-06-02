using Domain.Entities;
using InfraStructure.Interfaces;
using InfraStructure.Persistence;
using InfraStructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace InfraStructure.Repositories.Specific;

public class Order_itemsRepository:MainRepository<Order_items>,IOrder_itemsRepository
{
    private readonly AppdbContext _Context;
    public Order_itemsRepository(AppdbContext appdbContext) : base(appdbContext) => this._Context = appdbContext;
    public async override Task Update(Order_items t)
    {
        await base.Update(t);
        var order = await _Context.Orders
            .Include(o => o.Order_Items)
            .ThenInclude(o => o.Product)
            .FirstOrDefaultAsync(o => o.Id == t.OrderId);

        if (order != null)
        {
            order.clac_TotalPrice();
            await _Context.SaveChangesAsync();
        }
    }
    public override async Task Create(Order_items t)
    {
        await base.Create(t);

        var order = await _Context.Orders
            .Include(o => o.Order_Items)
            .ThenInclude(o=> o.Product)
            .FirstOrDefaultAsync(o => o.Id == t.OrderId);

        if (order != null)
        {
            order.clac_TotalPrice();
            await _Context.SaveChangesAsync();
        }
    }

}
