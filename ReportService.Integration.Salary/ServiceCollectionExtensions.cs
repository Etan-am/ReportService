using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;


namespace ReportService.Integration.Salary
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSalaryServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SalaryOption>(s => configuration.GetSection("Salary").Bind(s));
            services.AddScoped<ISalaryService, SalaryService>();
            return services;
        }
    }
}