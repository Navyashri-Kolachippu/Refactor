using ApiRefactor.Repositories;
using ApiRefactor.Repositories.Interfaces;
using ApiRefactor.Services;
using ApiRefactor.Services.Interfaces;

namespace ApiRefactor.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services)
        {

            services.AddScoped<IWaveRepository, WaveRepository>();

            services.AddScoped<IWaveService, WaveService>();

            return services;
        }
    }
}
