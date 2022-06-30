using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ReportService.Domain;
using ReportService.Integration.Buh;
using ReportService.Integration.Salary;

namespace ReportService.DataAccess.Services
{
    public interface IEnrichEmployeeService
    {
        Task<List<EnrichedEmployees>> Enrich(IEnumerable<Employee> employees);
    }

    public class EnrichEmployeeService : IEnrichEmployeeService
    {
        private readonly ISalaryService salaryService;
        private readonly IBuhService buhService;
        private readonly ILogger<EnrichEmployeeService> logger;

        public EnrichEmployeeService(ISalaryService salaryService,
            IBuhService buhService, ILogger<EnrichEmployeeService> logger)
        {
            this.salaryService = salaryService;
            this.buhService = buhService;
            this.logger = logger;
        }

        public async Task<List<EnrichedEmployees>> Enrich(IEnumerable<Employee> employees)
        {
            var result = new List<EnrichedEmployees>();
            foreach (var employee in employees)
            {
                var buhCode = await buhService.GetBuhCodeByInnAsync(employee.Inn);
                if (string.IsNullOrEmpty(buhCode))
                {
                    logger.LogWarning("For employee with inn: {employeeInn} buhCode not found", employee.Inn);
                    continue;
                }

                var salary = await salaryService.GetSalaryByBuhCodeAsync(buhCode);
                if (!salary.HasValue)
                {
                    logger.LogWarning("For employee with inn: {employeeInn} and buhCode: {buhCode} salary not found",
                        employee.Inn, buhCode);
                    continue;
                }

                result.Add(new EnrichedEmployees(employee, salary.Value));
            }

            return result;
        }
    }
}