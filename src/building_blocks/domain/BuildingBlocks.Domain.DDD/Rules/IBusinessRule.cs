namespace BuildingBlocks.Domain.DDD.Rules
{
    public interface IBusinessRule
    {
        string Message { get; }
        bool IsBroken();
    }
}