using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Registration.Application;
using Registration.Messages;

namespace Registration.Infrastructure
{
    public class RegistrationQueue : IRegistrationQueue
    {
        private readonly ISendEndpointProvider _provider;
        private readonly ILogger<RegistrationQueue> _logger;

        public RegistrationQueue(ISendEndpointProvider provider, ILogger<RegistrationQueue> logger)
        {
            _provider = provider;
            _logger = logger;
        }
        
        public async Task Enqueue(RegistrationRequest message, CancellationToken cancellationToken)
        {
            var endpoint = await _provider.GetSendEndpoint(Constants.QueueUri);

            _logger.LogInformation($"Enqueuing registration for {message.Name}");
            
            await endpoint.Send(message, cancellationToken);
        }
    }
}