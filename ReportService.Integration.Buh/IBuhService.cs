using System.Threading.Tasks;

namespace ReportService.Integration.Buh
{
    public interface IBuhService
    {
        Task<string> GetBuhCodeByInnAsync(string inn);
    }
}