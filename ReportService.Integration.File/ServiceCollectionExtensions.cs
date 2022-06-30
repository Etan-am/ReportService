using Microsoft.Extensions.DependencyInjection;

namespace ReportService.Integration.File
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFileServices(this IServiceCollection services)
        {
            services.AddScoped<IFileService, FileService>();
            return services;
        }
    }
}