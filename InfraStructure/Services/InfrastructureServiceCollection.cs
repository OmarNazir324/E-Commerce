using Application.Interfaces.HashBase;
using InfraStructure.Authentication;
using InfraStructure.Persistence.UnitOfWork;
using InfraStructure.Repositories.Generic;
using InfraStructure.Repositories.Specific;
using InfraStructure.Repositories.Specific.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace InfraStructure.Services
{
    public static class InfrastructureServiceCollection
    {
        public static IServiceCollection AddInfrastructureServiceCollection(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IOrder_itemsRepository, Order_itemsRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IHashBase, HashBase>();
            return services;
        }
    }
}
