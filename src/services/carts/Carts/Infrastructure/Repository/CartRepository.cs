using System.Threading.Tasks;
using BuildingBlocks.Infrastructure.AccountContext;
using Carts.Domain;
using Carts.Infrastructure.Json;
using Dapper;

namespace Carts.Infrastructure.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly IDbConnectionFactory _db;
        private readonly IAccountContextGetter _context;

        public CartRepository(IDbConnectionFactory db, IAccountContextGetter context)
        {
            _db = db;
            _context = context;
        }

        public async Task<Cart?> GetByIdAsync(string cartId)
        {
            var accountId = _context.GetAccountId();
            using var db = _db.GetConnection();
            var sql = $"SELECT {Constants.JsonColumn} FROM {Constants.TableName} WHERE {Constants.AccountIdColumn} = @accountId AND {Constants.CartIdColumn} = @cartId";
            var json = await db.QuerySingleOrDefaultAsync<string>(sql, new { accountId, cartId});
            return json.FromJson<Cart>();
        }

        public async Task SaveAsync(Cart cart)
        {
            var cartId = cart.CartId;
            var accountId = _context.GetAccountId();
            using var db = _db.GetConnection();
            var sql = $"INSERT INTO {Constants.TableName} ({Constants.AccountIdColumn}, {Constants.CartIdColumn}, {Constants.JsonColumn}) VALUES (@accountId, @cartId, @json::jsonb)";
            var json = cart.ToJson();
            await db.ExecuteAsync(sql, new {accountId, cartId, json});
        }
        
        public async Task UpdateAsync(Cart cart)
        {
            var cartId = cart.CartId;
            var accountId = _context.GetAccountId();
            using var db = _db.GetConnection();
            var sql = $"UPDATE {Constants.TableName} SET {Constants.JsonColumn} = @json::jsonb WHERE {Constants.AccountIdColumn} = @accountId AND {Constants.CartIdColumn} = @cartId";
            var json = cart.ToJson();
            await db.ExecuteAsync(sql, new {json, accountId, cartId});
        }
    }
}