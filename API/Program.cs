using API.APIServices;
using API.Extensions;
using API.Options;
using Application.Features.ProductFeature.DTOs;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using InfraStructure.Persistence;
using InfraStructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                                   .AddJsonFile("appsettings.json")
                                   .Build();
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var configBuilder = new ConfigurationBuilder()
                                 .SetBasePath(Directory.GetCurrentDirectory())
                                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                 .AddJsonFile($"appsettings.{env}.json", true, true);

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.ConfigureOptions<DataBaseOptionsSetup>();
            var serviceprovider = builder.Services.BuildServiceProvider().GetService<IOptions<DataBaseOptions>>()!.Value;

            builder.Services.AddControllers();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                   .AddJwtBearer(options =>
                   {
                       options.SaveToken = true;
                       options.RequireHttpsMetadata = false;
                       options.TokenValidationParameters = new TokenValidationParameters()
                       {
                           ValidateIssuer = true,
                           ValidateAudience = true,
                           ValidAudience = serviceprovider.ValidAudience,
                           ValidIssuer = serviceprovider.ValidIssuer,
                           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(serviceprovider.Secret))
                       };
                   });

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .WriteTo.Console()
                .WriteTo.File(
                    "Logs/log-.txt",
                    rollingInterval: RollingInterval.Day)

                .CreateLogger();
            builder.Host.UseSerilog();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            builder.Services.AddAutoMapper(cfg => { },
                typeof(CreateProductDTO).Assembly);
            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServiceCollection();
            builder.Services.AddAPIService(configuration,serviceprovider);

            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter Token"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
            builder.Services.AddDbContext<AppdbContext>(
                (ServiceProvider, DbContextOptionsBuilder) =>
            {
                var DatabaseOptions = ServiceProvider.GetService<IOptions<DataBaseOptions>>()!.Value;
                
                DbContextOptionsBuilder.UseSqlServer(DatabaseOptions.ConnectionString, sqloptions =>
                {
                    sqloptions.CommandTimeout(DatabaseOptions.CommandTimeOut);
                   // sqloptions.EnableRetryOnFailure(DatabaseOptions.RetryOnFailure);
                });
                DbContextOptionsBuilder.EnableDetailedErrors(DatabaseOptions.EnableDetailedErrors);
                DbContextOptionsBuilder.EnableSensitiveDataLogging(DatabaseOptions.EnableSenstiveDataLogging);
            });
            builder.Services
                .AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppdbContext>()
                .AddDefaultTokenProviders();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddAuthorization();

            var app = builder.Build();
            app.UseGlobalExceptionMiddleware();

            if (app.Environment.IsDevelopment())
            {
                // app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.MapControllers();
            app.UseSerilogRequestLogging();
            app.Run();
        }
    }
}
