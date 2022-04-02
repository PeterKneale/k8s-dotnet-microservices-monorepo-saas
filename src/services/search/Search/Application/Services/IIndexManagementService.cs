namespace Search.Application.Services
{
    public interface IIndexManagementService
    {
        void EnsureIndexExists();
        void ReCreateIndex();
    }
}