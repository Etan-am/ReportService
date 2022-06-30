using System.Data;

namespace ReportService.DataAccess.Providers
{
    public interface IConnectionProvider
    {
        IDbConnection GetConnection();
    }
}