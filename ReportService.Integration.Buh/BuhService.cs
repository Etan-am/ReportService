using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RestSharp;
using static System.GC;

namespace ReportService.Integration.Buh
{
    public class BuhService : IDisposable, IBuhService
    {
        private readonly RestClient client;

        public BuhService(IOptions<BuhOption> options)
        {
            client = new RestClient(options.Value.BaseUrl);
        }

        public async Task<string> GetBuhCodeByInnAsync(string inn)
        {
            try
            {
                var path = $"/api/inn/{inn}";
                var request = new RestRequest(path);
                var response = await client.GetAsync<string>(request);
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