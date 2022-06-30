using System.Data;
using Microsoft.Extensions.Options;
using Npgsql;

namespace ReportService.DataAccess.Providers
{
    public class PostgresqlConnectionProvider : IConnectionProvider
    {
        private readonly ConnectionStringOption option;

        public PostgresqlConnectionProvider(IOptions<ConnectionStringOption> option)
        {
            this.option = option.Value;
        }

        public IDbConnection GetConnection()
        {
            var connection = NpgsqlFactory.Instance.CreateConnection();
            if (connection != null)
            {
                connection.ConnectionString = BuildConnectionString();
            }

            return connection;
        }

        private string BuildConnectionString()
        {
            var connectionBuilder = new NpgsqlConnectionStringBuilder
            {
                Host = option.Server,
                Port = option.Port,
                Database = option.Database,
                Username = option.User,
                Password = option.Password,
                Timeout = option.Timeout,
                CommandTimeout = option.CommandTimeout,
            };
            return connectionBuilder.ToString();
        }
    }
}