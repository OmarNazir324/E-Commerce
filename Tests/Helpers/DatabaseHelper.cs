
using API;
using Domain.Entities;
using InfraStructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Helpers;

public static class DatabaseHelper
{
    public static String GetDBName(String testname) => testname + Guid.NewGuid().ToString();
    public static void ResetDB(CustomWebApplicationFactory<Program> factory)
    {
        var scope = factory.Services.CreateScope();
        var context  = scope.ServiceProvider.GetRequiredService<AppdbContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }
}
