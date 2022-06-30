using System;
using NUnit.Framework;
using ReportService.Api.Helpers;

namespace ReportService.Tests
{
    public class HelperTests
    {
        [Test]
        [TestCase(2019, 1, "Январь 2019")]
        [TestCase(1992, 1, "Январь 1992")]
        [TestCase(2019, 12, "Декабрь 2019")]
        public void GetFormattedReportDateTest(int year, int month, string result)
        {
            var date = ReportDateNameResolver.GetFormattedReportDate(year, month);
            Assert.AreEqual(date, result);
        }

        [Test]
        [TestCase(2019, 13)]
        [TestCase(2019, 0)]
        [TestCase(0, 1)]
        public void GetFormattedReportDateExceptionTest(int year, int month)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                ReportDateNameResolver.GetFormattedReportDate(year, month);
            });
        }
    }
}