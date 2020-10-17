using System.Data;

namespace Data.DbContexts.SqlServer.Core
{
    public interface IDbConnectionFactory
	{
		IDbConnection CreateConnection();
	}
}
