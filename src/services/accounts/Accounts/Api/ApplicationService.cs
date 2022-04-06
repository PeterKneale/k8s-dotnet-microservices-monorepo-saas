using System;
using System.Linq;
using System.Threading.Tasks;
using BuildingBlocks.Application.Exceptions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Accounts.Application;
using Accounts.Domain;
using BuildingBlocks.Infrastructure.AccountContext;

namespace Accounts.Api
{
    public class AccountsApplicationService : AccountsApplicationApi.AccountsApplicationApiBase
    {
        private readonly IMediator _mediator;
        private readonly IReadRepository _reader;
        private readonly string _accountId;

        public AccountsApplicationService(IMediator mediator, IReadRepository reader, IAccountContextGetter account)
        {
            _mediator = mediator;
            _reader = reader;
            _accountId = account.GetAccountId() ?? throw new Exception("Account context is required");
        }

        public override async Task<Empty> AddUser(AddUserRequest request, ServerCallContext context)
        {
            await _mediator.Send(new AddUser.Command(request.UserId, request.Email));
            return new Empty();
        }
        
        public override async Task<Account> GetAccount(GetAccountRequest request, ServerCallContext context)
        {
            var account = await _reader.GetAccountByIdAsync(_accountId);
            if (account == null)
            {
                throw new NotFoundException(nameof(Account), _accountId);
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