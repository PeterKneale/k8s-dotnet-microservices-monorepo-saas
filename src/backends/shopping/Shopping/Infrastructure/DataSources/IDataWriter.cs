using System.Threading.Tasks;
using Shopping.Application.Data;

namespace Shopping.Infrastructure.DataSources
{
    public interface IDataWriter
    {
        Task SaveAsync(StoreData store);  
        Task SaveAsync(AccountData account);
        Task SaveAsync(ProductData product);
    }

}