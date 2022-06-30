using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using ReportService.Domain;
using ReportService.Integration.File;

namespace ReportService.Tests.Integrations
{
    public class FileServiceTests
    {
        private FileService fileService;
        private string fileName;
        private string formattedReportDate;
        private IList<EnrichedEmployees> enrichedEmployees;

        [SetUp]
        public async Task Setup()
        {
            fileService = new FileService(NullLogger<FileService>.Instance);
            formattedReportDate = "Январь 2019";
            fileName = $"{2022}_{6}_report_{DateTime.Now.Ticks}.pdf";

            enrichedEmployees = new List<EnrichedEmployees>();
            var employeesFile = await File.ReadAllTextAsync("TestFiles/employee.json");
            var employees = JsonSerializer.Deserialize<Employees>(employeesFile);
            enrichedEmployees = employees.FileEmployees.Select(e =>
                    new EnrichedEmployees(new Domain.Employee { Department = e.Department, Name = e.Name }, e.Salary))
                .ToList();
        }

        [Test]
        public async Task CreateReportTest()
        {
            var result = await fileService.CreateReport(fileName, formattedReportDate,
                (IReadOnlyCollection<EnrichedEmployees>)enrichedEmployees);
            Assert.NotNull(result);
        }
    }

    public class Employee
    {
        [JsonPropertyName("name")] public string Name { get; set; }

        [JsonPropertyName("salary")] public decimal Salary { get; set; }

        [JsonPropertyName("department")] public string Department { get; set; }
    }

    public class Employees
    {
        [JsonPropertyName("employees")] public List<Employee> FileEmployees { get; set; }
    }
}