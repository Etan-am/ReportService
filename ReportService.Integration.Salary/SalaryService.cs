using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RestSharp;
using static System.GC;

namespace ReportService.Integration.Salary
{
    public class SalaryService : IDisposable, ISalaryService
    {
        private readonly RestClient client;

        public SalaryService(IOptions<SalaryOption> options)
        {
            client = new RestClient(options.Value.BaseUrl);
        }

        public async Task<decimal?> GetSalaryByBuhCodeAsync(string buhCode)
        {
            try
            {
                const string salaryPath = "/api/buhcode";
                var request = new RestRequest(salaryPath);
                request.AddJsonBody(buhCode);
                var response = await client.PostAsync<decimal?>(request);
                return response;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void Dispose()
        {
            SuppressFinalize(this);
        }
    }
}