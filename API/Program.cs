using API.Extensions;
using API.Options;
using Application.Features.ProductFeature.DTOs;
using Application.Services;
using Domain.Entities;
using InfraStructure.Persistence;
using InfraStructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.ConfigureOptions<DataBaseOptionsSetup>();
            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            //builder.Services.AddOpenApi();
            builder.Services.AddAutoMapper(typeof(CreateProductDTO).Assembly);
            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServiceCollection();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<AppdbContext>(
                (ServiceProvider, DbContextOptionsBuilder) =>
            {
                var DatabaseOptions = ServiceProvider.GetService<IOptions<DataBaseOptions>>()!.Value;
                
                DbContextOptionsBuilder.UseSqlServer(DatabaseOptions.ConnectionString, sqloptions =>
                {
                    sqloptions.CommandTimeout(DatabaseOptions.CommandTimeOut);
                    sqloptions.EnableRetryOnFailure(DatabaseOptions.RetryOnFailure);
                });
                DbContextOptionsBuilder.EnableDetailedErrors(DatabaseOptions.EnableDetailedErrors);
                DbContextOptionsBuilder.EnableSensitiveDataLogging(DatabaseOptions.EnableSenstiveDataLogging);
            });
            builder.Services
                .AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppdbContext>()
                .AddDefaultTokenProviders();
            builder.Services.AddEndpointsApiExplorer();
            var app = builder.Build();
            app.UseGlobalExceptionMiddleware();

            if (app.Environment.IsDevelopment())
            {
                // app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI();
            app.MapControllers();

            app.Run();
        }
    }
}
