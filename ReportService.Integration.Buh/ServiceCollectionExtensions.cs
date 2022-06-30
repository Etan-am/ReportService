using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ReportService.Integration.Buh
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBuhServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<BuhOption>(s => configuration.GetSection("Buh").Bind(s));
            services.AddScoped<IBuhService, BuhService>();
            return services;
        }
    }
}