using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using iText.Kernel.Exceptions;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.Extensions.Logging;
using ReportService.Domain;

namespace ReportService.Integration.File
{
    public class FileService : IFileService
    {
        private readonly ILogger<FileService> logger;
        private static readonly Paragraph NewLine = new Paragraph(new Text(Environment.NewLine));
        private static readonly LineSeparator LineSeparator = new LineSeparator(new SolidLine());
        private static readonly Tab Tab = new Tab();


        private readonly PdfFont font = PdfFontFactory.CreateFont("Fonts/Roboto-Thin.ttf", "cp1251",
            PdfFontFactory.EmbeddingStrategy.FORCE_EMBEDDED);

        public FileService(ILogger<FileService> logger)
        {
            this.logger = logger;
        }

        public async Task<string> CreateReport(string fileName, string formattedReportDate,
            IReadOnlyCollection<EnrichedEmployees> enrichedEmployees)
        {
            try
            {
                var filePath = $"TestFiles/{fileName}";
                await using var writer = new PdfWriter(filePath);
                using var pdf = new PdfDocument(writer);
                using var document = new Document(pdf).SetFont(font);
                var pageSize = document.GetPdfDocument().GetDefaultPageSize();
                var effectivePageSize = document.GetPageEffectiveArea(pageSize);
                var rightTabStopPoint = effectivePageSize.GetWidth() / 2;

                document.Add(new Paragraph(formattedReportDate).SetBold());
                document.Add(NewLine);
                document.Add(LineSeparator);
                foreach (var department in enrichedEmployees.Select(e => e.Employee.Department).Distinct())
                {
                    document.Add(new Paragraph(department));
                    document.Add(NewLine);
                    var departmentEmployee = enrichedEmployees.Where(e => e.Employee.Department == department).ToList();
                    foreach (var employee in departmentEmployee)
                    {
                        var blockElement = new Paragraph();
                        blockElement.Add($"{employee.Employee.Name}");
                        blockElement.Add(Tab);
                        blockElement.Add($"{employee.Salary:F} ₽");
                        blockElement.AddTabStops(new TabStop(rightTabStopPoint, TabAlignment.LEFT));
                        document.Add(blockElement);
                       // document.Add(NewLine);
                    }
                    document.Add(NewLine);
                    document.Add(
                        new Paragraph($"Всего по отделу").Add(Tab).Add($"{departmentEmployee.Sum(e => e.Salary):F} ₽")
                            .AddTabStops(new TabStop(rightTabStopPoint, TabAlignment.LEFT)));
                    document.Add(LineSeparator);
                }

                document.Add(
                    new Paragraph($"Всего по предприятию").Add(Tab).Add($"{enrichedEmployees.Sum(e => e.Salary):F} ₽")
                        .AddTabStops(new TabStop(rightTabStopPoint, TabAlignment.LEFT)));
                document.Close();
                return filePath;
            }
            catch (PdfException pdfException)
            {
                logger.LogError(pdfException, "Create report error");
                return null;
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error");
                throw;
            }
        }
    }
}