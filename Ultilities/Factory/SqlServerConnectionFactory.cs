using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultilities.Core;

namespace Ultilities.Factory
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
