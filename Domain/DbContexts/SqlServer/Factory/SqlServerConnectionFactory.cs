using Data.DbContexts.SqlServer.Core;
using System.Data;
using System.Data.SqlClient;

namespace Data.DbContexts.SqlServer.Factory
{
    public class SqlServerConnectionFactory : IDbConnectionFactory
    {
        string _connectionString = null;

		public SqlServerConnectionFactory(string connectionString)
		{
			_connectionString = connectionString;
		}

		public IDbConnection CreateConnection()
		{
			return new SqlConnection(_connectionString);
		}
	}
}
