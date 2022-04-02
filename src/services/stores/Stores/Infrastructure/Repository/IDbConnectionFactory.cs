using System.Data;

namespace Stores.Infrastructure.Repository
{
    public interface IDbConnectionFactory
    {
        IDbConnection GetConnection();
    }
}