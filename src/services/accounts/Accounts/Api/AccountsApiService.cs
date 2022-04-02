using System.Linq;
using System.Threading.Tasks;
using BuildingBlocks.Application.Exceptions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Accounts.Application;
using Accounts.Domain;

namespace Accounts.Api
{
    public class AccountsApiService : AccountsApi.AccountsApiBase
    {
        private readonly IMediator _mediator;
        private readonly IReadRepository _reader;

        public AccountsApiService(IMediator mediator, IReadRepository reader)
        {
            _mediator = mediator;
            _reader = reader;
        }

        public override async Task<Empty> AddAccount(AddAccountRequest request, ServerCallContext context)
        {
            await _mediator.Send(new AddAccount.Command(request.AccountId, request.Name));
            return new Empty();
        }

        public override async Task<Empty> AddUser(AddUserRequest request, ServerCallContext context)
        {
            await _mediator.Send(new AddUser.Command(request.UserId, request.Email));
            return new Empty();
        }

        public override async Task<Empty> VerifyUser(VerifyUserRequest request, ServerCallContext context)
        {
            await _mediator.Send(new VerifyUser.Command(request.Email, request.Token, request.Password));
            return new Empty();
        }

        public override async Task<Account> GetAccount(GetAccountRequest request, ServerCallContext context)
        {
            var account = await _reader.GetAccountByIdAsync(request.AccountId);
            if (account == null)
            {
                throw new NotFoundException(nameof(Account), request.AccountId);
            }
            return new Account {AccountId = account.AccountId, Name = account.Name};
        }

        public override async Task<User> GetUser(GetUserRequest request, ServerCallContext context)
        {
            var user = await _reader.GetUserByIdAsync(request.UserId);
            if (user == null)
            {
                throw new NotFoundException(nameof(User), request.UserId);
            }
            return new User {UserId = user.UserId, Email = user.Email};
        }

        // todo; add service to service role check (admin only)
        public override async Task<EmailVerificationToken> GetEmailVerificationToken(GetEmailVerificationTokenRequest request, ServerCallContext context)
        {
            var user = await _reader.GetUserByIdAsync(request.UserId);
            if (user == null)
            {
                throw new NotFoundException(nameof(User), request.UserId);
            }
            return new EmailVerificationToken {Token = user.EmailVerificationToken};
        }

        // todo; add service to service role check (admin only)
        public override async Task<AccountsReply> ListAccounts(ListAccountRequest request, ServerCallContext context)
        {
            var accounts = await _reader.ListAccounts();
            return new AccountsReply
            {
                Accounts = {accounts.Select(x => new Account {AccountId = x.AccountId, Name = x.Name})}
            };
        }
        public override async Task<UsersReply> ListUsers(ListUsersRequest request, ServerCallContext context)
        {
            var users = await _reader.ListUsers();
            return new UsersReply
            {
                Users = {users.Select(x => new User {UserId = x.UserId, Email = x.Email})}
            };
        }
    }
}