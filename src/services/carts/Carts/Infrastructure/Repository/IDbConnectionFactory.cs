using System.Data;

namespace Carts.Infrastructure.Repository
{
    public interface IDbConnectionFactory
    {
        IDbConnection GetConnection();
    }
}