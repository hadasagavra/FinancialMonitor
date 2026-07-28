using Microsoft.Extensions.DependencyInjection;
using Repository.Interfaces;

namespace Repository.Repositories
{
    public static class ExtensionRepository
    {
        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            return services;
        }
    }
}