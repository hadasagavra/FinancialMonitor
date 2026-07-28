using Microsoft.Extensions.DependencyInjection;
using Service.Interfaces;
using Service.Mapping;

namespace Service.Services
{
    public static class ExtensionService
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddScoped<ITransactionService, TransactionService>();
            return services;
        }
    }
}