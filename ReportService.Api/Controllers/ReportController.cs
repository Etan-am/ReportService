using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReportService.Api.Helpers;
using ReportService.DataAccess.Repositories;
using ReportService.DataAccess.Services;
using ReportService.Integration.File;

namespace ReportService.Api.Controllers
{
    [Route("api/[controller]")]
    public class ReportController : Controller
    {
        private readonly IEmployeeRepository employeeRepository;
        private readonly IFileService fileService;
        private readonly IEnrichEmployeeService enrichEmployeeService;
        private const string ContentTypePdf = "application/pdf";

        public ReportController(IEmployeeRepository employeeRepository, IFileService fileService,
            IEnrichEmployeeService enrichEmployeeService)
        {
            this.employeeRepository = employeeRepository;
            this.fileService = fileService;
            this.enrichEmployeeService = enrichEmployeeService;
        }

        [HttpGet]
        [Route("{year:int}/{month:int}")]
        public async Task<IActionResult> Download(int year, int month)
        {
            var formattedReportDate = ReportDateNameResolver.GetFormattedReportDate(year, month);
            var fileName = $"{year}_{month}_report_{DateTime.Now.Ticks}.pdf";

            var employees = (await employeeRepository.GetEmployees()).ToList();
            var enrichedEmployees = await enrichEmployeeService.Enrich(employees);

            var filePath = await fileService.CreateReport(fileName, formattedReportDate, enrichedEmployees);
            if (string.IsNullOrEmpty(filePath))
                return new StatusCodeResult(500);

            var file = await System.IO.File.ReadAllBytesAsync(filePath);
            var response = File(file, ContentTypePdf, fileName);
            return response;
        }
    }
}