using System.Threading.Tasks;
using MassTransit;
using Shopping.Infrastructure.DataSources;
using Accounts.Messages;
using Shopping.Application.Data;

namespace Shopping.Infrastructure.Consumers
{
    public class AccountAddedConsumer : IConsumer<AccountAdded>
    {
        private readonly IDataWriter _writer;

        public AccountAddedConsumer(IDataWriter writer)
        {
            _writer = writer;
        }

        public async Task Consume(ConsumeContext<AccountAdded> context)
        {
            await _writer.SaveAsync(new AccountData
            {
                AccountId = context.Message.AccountId,
                Name = context.Message.Name
            });
        }
    }
}