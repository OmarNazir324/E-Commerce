using InfraStructure.Interfaces;
using InfraStructure.Persistence.UnitOfWork;
using InfraStructure.Repositories.Generic;
using InfraStructure.Repositories.Specific;
using Microsoft.Extensions.DependencyInjection;

namespace InfraStructure.Services
{
    public static class InfrastructureServiceCollection
    {
        public static IServiceCollection AddInfrastructureServiceCollection(this IServiceCollection services)
        {
            services.AddScoped(typeof(IMainInterFace<>), typeof(MainRepository<>));
            services.AddScoped<IOrder_itemsRepository, Order_itemsRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
