using System.Threading;
using System.Threading.Tasks;
using Registration.Messages;

namespace Registration.Application
{
    public interface IRegistrationQueue
    {
        Task Enqueue(RegistrationRequest message, CancellationToken cancellationToken);
    }
}