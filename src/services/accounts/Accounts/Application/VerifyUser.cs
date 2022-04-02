using System;
using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Application.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Accounts.Domain;

namespace Accounts.Application
{
    public static class VerifyUser
    {
        public record Command(string Email, string Token, string Password) : IRequest;

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Email).NotEmpty().MaximumLength(100);
                RuleFor(x => x.Token).NotEmpty().MaximumLength(36);
                RuleFor(x => x.Password).NotEmpty().MaximumLength(100);
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly IWriteRepository _writer;
            private readonly IReadRepository _reader;
            private readonly ILogger<Handler> _logs;

            public Handler(IWriteRepository writer, IReadRepository reader,ILogger<Handler> logs)
            {
                _writer = writer;
                _reader = reader;
                _logs = logs;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var token = request.Token;
                var email = request.Email;
                var password = request.Password;

                var user = await _reader.GetUserByEmailAsync(email);
                if (user == null)
                {
                    throw new NotFoundException(nameof(User), email);
                }
                
                user.VerifyEmail(token);
                user.SetPassword(password);

                await _writer.SaveAsync(user);

                return Unit.Value;
            }
        }
    }

}