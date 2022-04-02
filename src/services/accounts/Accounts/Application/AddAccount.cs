using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Accounts.Domain;
using Accounts.Messages;

namespace Accounts.Application
{
    public static class AddAccount
    {
        public record Command(string AccountId, string Name) : IRequest;

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.AccountId).NotEmpty().MaximumLength(36);
                RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly IWriteRepository _writer;
            private readonly IReadRepository _reader;
            private readonly IPublishEndpoint _publisher;
            private readonly ILogger<Handler> _logs;

            public Handler(IWriteRepository writer, IReadRepository reader, IPublishEndpoint publisher, ILogger<Handler> logs)
            {
                _writer = writer;
                _reader = reader;
                _publisher = publisher;
                _logs = logs;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var accountId = request.AccountId;
                var name = request.Name;

                var account = await _reader.GetAccountByIdAsync(accountId);
                if (account != null)
                {
                    return Unit.Value;
                }
                
                _logs.LogInformation($"Creating account {accountId} {name}");

                account = new Account(accountId, name);

                await _writer.SaveAsync(account);
                await _publisher.Publish(new AccountAdded(account.AccountId, request.Name), cancellationToken);

                return Unit.Value;
            }
        }
    }

}