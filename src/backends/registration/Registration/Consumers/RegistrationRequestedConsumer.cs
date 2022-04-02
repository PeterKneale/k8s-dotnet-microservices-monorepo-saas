using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Registration.Application;
using Registration.Messages;

namespace Registration.Consumers
{
    public class RegistrationRequestedConsumer : IConsumer<RegistrationRequest>
    {
        private readonly IRegistrationService _service;
        private readonly ILogger<RegistrationRequestedConsumer> _logs;

        public RegistrationRequestedConsumer(IRegistrationService service, ILogger<RegistrationRequestedConsumer> logs)
        {
            _service = service;
            _logs = logs;
        }

        public async Task Consume(ConsumeContext<RegistrationRequest> context)
        {
            var accountId = context.Message.AccountId;
            var storeId = context.Message.StoreId;
            var userId = context.Message.StoreId;
            var name = context.Message.Name;
            var email = context.Message.Email;
            var cancellationToken = context.CancellationToken;

            _logs.LogInformation($"Handling registration for {name}");
            try
            {
                await _service.Register(accountId, storeId, userId, name, email, cancellationToken);
            }
            catch (Exception e)
            {
                _logs.LogError(e, $"Error handling registration for {name}");
                throw;
            }
        }
    }
}