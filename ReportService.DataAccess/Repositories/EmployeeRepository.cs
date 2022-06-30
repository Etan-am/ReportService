using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using ReportService.DataAccess.Extensions;
using ReportService.DataAccess.Providers;
using ReportService.Domain;

namespace ReportService.DataAccess.Repositories
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetEmployees();
    }

    public class EmployeeRepository : IDisposable, IEmployeeRepository
    {
        private readonly IConnectionProvider connectionProvider;

        public EmployeeRepository(IConnectionProvider connectionProvider)
        {
            this.connectionProvider = connectionProvider;
        }

        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            const string getEmployeesQuery =
                "SELECT e.id, e.name, e.inn, d.name as department from emps e inner join deps d on e.departmentid = d.id";
            return await connectionProvider.ExecuteAndDispose(dbConnection =>
                dbConnection.QueryAsync<Employee>(getEmployeesQuery));
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}