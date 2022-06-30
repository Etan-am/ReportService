using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NUnit.Framework;
using ReportService.DataAccess.Services;
using ReportService.Domain;
using ReportService.Integration.Buh;
using ReportService.Integration.Salary;

namespace ReportService.Tests.DataAccess
{
    public class EnrichEmployeeServiceTests
    {
        private EnrichEmployeeService enrichEmployeeService;
        private Mock<ISalaryService> mockSalaryService;
        private Mock<IBuhService> mockBuhService;
        private IList<Employee> employees;

        [SetUp]
        public void Setup()
        {
            mockSalaryService = new Mock<ISalaryService>();
            mockBuhService = new Mock<IBuhService>();
            mockBuhService.Setup(b => b.GetBuhCodeByInnAsync(It.IsAny<string>()))
                .Returns((string inn) => GetBuhCodeByInnValueFunction(inn));
            mockSalaryService.Setup(service => service.GetSalaryByBuhCodeAsync(It.IsAny<string>())).Returns(
                (string buhCode) =>
                    GetSalaryByBuhCodeValueFunction(buhCode));

            enrichEmployeeService = new EnrichEmployeeService(
                mockSalaryService.Object,
                mockBuhService.Object,
                new NullLogger<EnrichEmployeeService>());

            employees = new List<Employee>();
            employees.Add(new Employee
                { Id = Guid.NewGuid(), Name = "Андрей Сергеевич Бубнов", Inn = "1", Department = "ФинОтдел" });
            employees.Add(new Employee
                { Id = Guid.NewGuid(), Name = "Григорий Евсеевич Зиновьев", Inn = "2", Department = "ФинОтдел" });
            employees.Add(new Employee
                { Id = Guid.NewGuid(), Name = "Яков Михайлович Свердлов ", Inn = "3", Department = "ФинОтдел" });
            employees.Add(new Employee
                { Id = Guid.NewGuid(), Name = "Алексей Иванович Рыков", Inn = "4", Department = "ФинОтдел" });
            employees.Add(new Employee
                { Id = Guid.NewGuid(), Name = "Фрол Романович Козлов", Inn = "5", Department = "ИТ" });
            employees.Add(new Employee
                { Id = Guid.NewGuid(), Name = "Дмитрий Степанович Полянски ", Inn = "6", Department = "ИТ" });
            employees.Add(new Employee
                { Id = Guid.NewGuid(), Name = "Василий Васильевич Кузнецов", Inn = "7", Department = "Бухгалтерия" });
            employees.Add(new Employee
                { Id = Guid.NewGuid(), Name = "Демьян Сергеевич Коротченко", Inn = "8", Department = "Бухгалтерия" });
        }

        private Task<decimal?> GetSalaryByBuhCodeValueFunction(string buhCode)
        {
            return Task.FromResult(buhCode switch
            {
                "111" => (decimal?)11.22,
                "2" => (decimal?)90000.22,
                "3" => (decimal?)500000.33,
                "4" => (decimal?)30000.44,
                "5" => (decimal?)310000.55,
                "6" => (decimal?)230000.66,
                "7" => (decimal?)340000.77,
                _ => null
            });
        }

        private Task<string> GetBuhCodeByInnValueFunction(string inn)
        {
            return Task.FromResult(inn switch
            {
                "1" => "111",
                "6" => "",
                _ => inn
            });
        }

        [Test]
        [TestCase("1", 11.22)]
        [TestCase("2", 90000.22)]
        [TestCase("3", 500000.33)]
        [TestCase("4", 30000.44)]
        [TestCase("5", 310000.55)]
        [TestCase("6", null)]
        [TestCase("8", null)]
        public async Task GetFormattedReportDateTest(string inn, decimal? salary)
        {
            var result = await enrichEmployeeService.Enrich(employees);
            Assert.AreEqual(result.FirstOrDefault(e => e.Employee.Inn == inn)?.Salary, salary);
        }
    }
}