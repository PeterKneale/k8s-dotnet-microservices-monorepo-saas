using System.Linq;
using System.Threading.Tasks;
using Accounts.Application;
using Accounts.Domain;
using BuildingBlocks.Application.Exceptions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;

namespace Accounts.Api
{
    public class AccountsPlatformService : AccountsPlatformApi.AccountsPlatformApiBase
    {
        private readonly IMediator _mediator;
        private readonly IReadRepository _reader;

        public AccountsPlatformService(IMediator mediator, IReadRepository reader)
        {
            _mediator = mediator;
            _reader = reader;
        }

        public override async Task<Empty> ProvisionAccount(ProvisionAccountRequest request, ServerCallContext context)
        {
            await _mediator.Send(new AddAccount.Command(request.AccountId, request.Name));
            return new Empty();
        }

        public override async Task<Empty> VerifyUser(VerifyUserRequest request, ServerCallContext context)
        {
            await _mediator.Send(new VerifyUser.Command(request.Email, request.Token, request.Password));
            return new Empty();
        }

        public override async Task<EmailVerificationToken> GetEmailVerificationToken(GetEmailVerificationTokenRequest request, ServerCallContext context)
        {
            var user = await _reader.GetUserByIdAsync(request.UserId);
            if (user == null)
            {
                throw new NotFoundException(nameof(User), request.UserId);
            }
            return new EmailVerificationToken {Token = user.EmailVerificationToken};
        }

        public override async Task<AccountSummary> GetAccountSummary(GetAccountSummaryRequest request, ServerCallContext context)
        {
            var account = await _reader.GetAccountByIdAsync(request.AccountId);
            if (account == null)
            {
                throw new NotFoundException(nameof(Account), request.AccountId);
            }
            return new AccountSummary
            {
                AccountId = account.AccountId,
                Name = account.Name
            };
        }

        public override async Task<ListAccountSummariesReply> ListAccountSummaries(ListAccountSummariesRequest request, ServerCallContext context)
        {
            var accounts = await _reader.ListAccounts();
            return new ListAccountSummariesReply
            {
                Accounts =
                {
                    accounts.Select(x => new AccountSummary
                    {
                        AccountId = x.AccountId,
                        Name = x.Name
                    })
                }
            };
        }
    }
}