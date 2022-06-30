using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ReportService.DataAccess.Providers;
using ReportService.DataAccess.Repositories;

namespace ReportService.DataAccess.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDataAccess(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<ConnectionStringOption>(pg => configuration.GetSection("ConnectionStrings:Db").Bind(pg));
            services.TryAddScoped<IConnectionProvider, PostgresqlConnectionProvider>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            return services;
        }
    }
}