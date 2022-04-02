using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Accounts.Domain;
using BuildingBlocks.Infrastructure.AccountContext;

namespace Accounts.Application
{
    public static class AddUser
    {
        public record Command(string UserId, string Email) : IRequest;

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.UserId).NotEmpty().MaximumLength(36);
                RuleFor(x => x.Email).NotEmpty().MaximumLength(100);
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly IWriteRepository _writer;
            private readonly IReadRepository _reader;
            private readonly IAccountContextGetter _context;
            private readonly ILogger<Handler> _logs;

            public Handler(IWriteRepository writer, IReadRepository reader, IAccountContextGetter context, ILogger<Handler> logs)
            {
                _writer = writer;
                _reader = reader;
                _context = context;
                _logs = logs;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var accountId = _context.GetAccountId();
                var userId = request.UserId;
                var email = request.Email;

                var user = await _reader.GetUserByIdAsync(userId);
                if (user != null)
                {
                    return Unit.Value;
                }
                
                _logs.LogInformation($"Adding user {userId} {email} to account {accountId}");

                user = new User(accountId, userId, email);

                await _writer.SaveAsync(user);

                return Unit.Value;
            }
        }
    }

}