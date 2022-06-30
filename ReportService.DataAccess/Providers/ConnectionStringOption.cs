namespace ReportService.DataAccess.Providers
{
    public class ConnectionStringOption
    {
        public string Server { get; set; } = "localhost";
        public int Port { get; set; } = 5432;
        public string Database { get; set; } = "db";
        public string User { get; set; } = "postgres";
        public string Password { get; set; } = "dev";
        public int Timeout { get; set; } = 300;
        public int CommandTimeout { get; set; } = 60;
    }
}