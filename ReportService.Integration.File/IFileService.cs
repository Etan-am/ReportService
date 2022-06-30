using System.Collections.Generic;
using System.Threading.Tasks;
using ReportService.Domain;

namespace ReportService.Integration.File
{
    public interface IFileService
    {
        Task<string> CreateReport(string fileName, string formattedReportDate,
            IReadOnlyCollection<EnrichedEmployees> enrichedEmployees);
    }
}