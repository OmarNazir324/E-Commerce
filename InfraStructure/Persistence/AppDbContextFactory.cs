using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace InfraStructure.Persistence
{
    //public class AppDbContextFactory : IDesignTimeDbContextFactory<AppdbContext>
    //{
    //    //private readonly IConfiguration _configuration;
    //    //public AppDbContextFactory(IConfiguration configuration)
    //    //{
    //    //    this._configuration= configuration;
    //    //}
    //    //public AppdbContext CreateDbContext(string[] args)
    //    //{
    //    //    var optionsBuilder = new DbContextOptionsBuilder<AppdbContext>();

    //    //    optionsBuilder.UseSqlServer(
    //    //        _configuration.GetConnectionString("DefaultConnection")
    //    //        );
    //    //    return new AppdbContext(optionsBuilder.Options);
    //    //}
    //}
}