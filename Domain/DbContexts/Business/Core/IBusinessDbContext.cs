using System.Data;

namespace Data.DbContexts.Business.Core
{
    public interface IBusinessDbContext
    {
        IDbConnection CreateConnection();
    }
}
