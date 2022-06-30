using System;
using System.Data;
using ReportService.DataAccess.Providers;

namespace ReportService.DataAccess.Extensions
{
    public static class DbConnectionExtensions
    {
        public static TResult ExecuteAndDispose<TResult>(
            this IConnectionProvider postresqlConnectionProvider, Func<IDbConnection, TResult> execute)
        {
            using var connection = postresqlConnectionProvider.GetConnection();
            return execute(connection);
        }
    }
}