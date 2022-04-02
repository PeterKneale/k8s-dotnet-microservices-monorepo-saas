using System.Threading.Tasks;

namespace Accounts.Domain
{
    public interface IWriteRepository
    {
        Task SaveAsync(Account account);
        Task SaveAsync(User user);
    }
}