using API.APIServices;
using API.Extensions;
using API.Options;
using Application.DataBaseOptions;
using Application.Features.ProductFeature.DTOs;
using Application.Services;
using InfraStructure.Persistence;
using InfraStructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
namespace API
{
    public partial class Program
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
            builder.Services.ConfigureOptions<DataBaseOptionsSetup>();
            

            builder.Services.AddControllers();

            builder.Services.AddDbContext<AppdbContext>(
                (ServiceProvider, DbContextOptionsBuilder) =>
                {
                    var DatabaseOptions = ServiceProvider.GetService<IOptions<DataBaseOptions>>()!.Value;
                    DbContextOptionsBuilder.UseSqlServer(DatabaseOptions.ConnectionString, sqloptions =>
                    {
                        sqloptions.CommandTimeout(DatabaseOptions.CommandTimeOut);
                    });
                    DbContextOptionsBuilder.EnableDetailedErrors(DatabaseOptions.EnableDetailedErrors);
                    DbContextOptionsBuilder.EnableSensitiveDataLogging(DatabaseOptions.EnableSenstiveDataLogging);
                });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer();
            builder.Services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
                .Configure<IOptions<DataBaseOptions>>((jwt, dboptions) =>
                   {
                       var databaseoptions = dboptions.Value!;
                       jwt.SaveToken = true;
                       jwt.RequireHttpsMetadata = false;
                       jwt.TokenValidationParameters = new TokenValidationParameters()
                       {
                           ValidateIssuer = true,
                           ValidateAudience = true,
                           ValidAudience = databaseoptions.ValidAudience,
                           ValidIssuer = databaseoptions.ValidIssuer,
                           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(databaseoptions.Secret))
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
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddAutoMapper(cfg => { },
                typeof(CreateProductDto).Assembly);
            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServiceCollection();
            builder.Services.AddAPIService(configuration);
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
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddAuthorization();
            var app = builder.Build();
            app.UseGlobalExceptionMiddleware();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.MapControllers();
            app.UseSerilogRequestLogging();
            app.Run();
        }
    }
}
