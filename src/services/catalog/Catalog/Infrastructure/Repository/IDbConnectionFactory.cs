using System.Data;

namespace Catalog.Infrastructure.Repository
{
    public interface IDbConnectionFactory
    {
        IDbConnection GetConnection();
    }
}