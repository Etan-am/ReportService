using System;
using System.Globalization;

namespace ReportService.Api.Helpers
{
    public static class ReportDateNameResolver
    {
        public static string GetFormattedReportDate(int year, int month)
        {
            var formatProvider = CultureInfo.GetCultureInfo("ru-ru");
            return formatProvider.TextInfo.ToTitleCase(
                new DateTime(year, month, 1).ToString("MMMM yyyy", formatProvider));
        }
    }
}