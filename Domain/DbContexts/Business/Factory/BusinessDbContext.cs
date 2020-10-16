using Data.DbContexts.Business.Core;
using System.Data;
using System.Data.SqlClient;

namespace Data.DbContexts.Business.Factory
{
    public class BusinessDbContext : IBusinessDbContext
    {
        string _connectionString = null;

        public BusinessDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
