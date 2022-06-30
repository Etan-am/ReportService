using System.Threading.Tasks;

namespace ReportService.Integration.Salary
{
    public interface ISalaryService
    {
        Task<decimal?> GetSalaryByBuhCodeAsync(string buhCode);
    }
}