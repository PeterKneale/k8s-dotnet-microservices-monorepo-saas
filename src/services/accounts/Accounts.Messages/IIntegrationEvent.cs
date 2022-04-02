namespace Accounts.Messages
{
    public interface IIntegrationEvent
    {
        string AccountId { get; }
    }
}