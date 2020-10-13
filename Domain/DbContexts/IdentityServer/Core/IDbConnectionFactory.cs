using System.Data;

namespace Data.DbContexts.IdentityServer.Core
{
    public interface IDbConnectionFactory
	{
		IDbConnection CreateConnection();
	}
}
