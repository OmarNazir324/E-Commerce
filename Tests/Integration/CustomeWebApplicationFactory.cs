using InfraStructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    public String Database_Name { get; set; } = Guid.NewGuid().ToString();
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<AppdbContext>();
            services.RemoveAll<DbContextOptions<AppdbContext>>();
            services.RemoveAll(typeof(IDbContextOptionsConfiguration<AppdbContext>));

            services.AddDbContext<AppdbContext>(options =>
            {
                options.UseInMemoryDatabase(Database_Name);
            });
        });

    }
}