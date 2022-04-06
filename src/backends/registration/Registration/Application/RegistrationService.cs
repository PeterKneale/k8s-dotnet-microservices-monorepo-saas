using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Stores.Api;
using Accounts.Api;
using FluentValidation.Validators;

namespace Registration.Application
{
    public interface IRegistrationService
    {
        Task Register(string accountId, string storeId, string userId, string name, string email, CancellationToken cancellationToken);
    }

    public class RegistrationService : IRegistrationService
    {
        private readonly IAccountsServiceGateway _accounts;
        private readonly IStoresServiceGateway _stores;
        private readonly ILogger<RegistrationService> _log;

        public RegistrationService(IAccountsServiceGateway accounts, IStoresServiceGateway stores, ILogger<RegistrationService> log)
        {
            _accounts = accounts;
            _stores = stores;
            _log = log;
        }

        public async Task Register(string accountId, string storeId, string userId, string name, string email, CancellationToken cancellationToken)
        {
            _log.LogInformation($"Processing registration for {name}");

            // TODO: Provision simply for now, use a saga for orchestration later
            var task1 = _accounts.ProvisionAccountAsync(new ProvisionAccountRequest
            {
                AccountId = accountId,
                UserId = userId,
                Name = name,
                Email = email,
            }, cancellationToken);

            var task2 = _stores.AddStoreAsync(accountId, new AddStoreRequest {StoreId = storeId, Name = name}, cancellationToken);

            await Task.WhenAll(task1, task2);
        }
    }
}