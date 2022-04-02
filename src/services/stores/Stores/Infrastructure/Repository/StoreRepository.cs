using System.Collections.Generic;
using System.Threading.Tasks;
using BuildingBlocks.Infrastructure.AccountContext;
using Dapper;
using Stores.Domain;

namespace Stores.Infrastructure.Repository
{
    public class StoreRepository : IStoreRepository
    {
        private readonly IDbConnectionFactory _db;
        private readonly IAccountContextGetter _context;

        public StoreRepository(IDbConnectionFactory db, IAccountContextGetter context)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;

            _db = db;
            _context = context;
        }

        public async Task<Store?> GetByIdAsync(string storeId)
        {
            var accountId = _context.GetAccountId();
            using var db = _db.GetConnection();
            var sql = $"SELECT * FROM {SchemaConstants.TableStore} WHERE {SchemaConstants.AccountId} = @accountId AND {SchemaConstants.StoreId} = @storeId";
            return await db.QuerySingleOrDefaultAsync<Store>(sql, new {accountId, storeId});
        }

        public async Task<IEnumerable<Store>> ListAsync()
        {
            using var db = _db.GetConnection();
            var sql = $"SELECT * FROM {SchemaConstants.TableStore}";
            return await db.QueryAsync<Store>(sql);
        }

        public async Task<Store?> GetByDomainAsync(string domain)
        {
            using var db = _db.GetConnection();
            var sql = $"SELECT * FROM {SchemaConstants.TableStore} WHERE {SchemaConstants.Domain} = @domain";
            return await db.QuerySingleOrDefaultAsync<Store>(sql, new {domain});
        }

        public async Task<Store?> GetBySubdomainAsync(string subdomain)
        {
            using var db = _db.GetConnection();
            var sql = $"SELECT * FROM {SchemaConstants.TableStore} WHERE {SchemaConstants.Subdomain} = @subdomain";
            return await db.QuerySingleOrDefaultAsync<Store>(sql, new {subdomain});
        }

        public async Task<bool> ExistsByIdAsync(string storeId) => await GetByIdAsync(storeId) != null;

        public async Task<bool> ExistsDefaultDomainAsync(string defaultDomain) => await GetByDomainAsync(defaultDomain) != null;

        public async Task CreateAsync(Store store)
        {
            var accountId = _context.GetAccountId();
            using var db = _db.GetConnection();
            var sql = $"INSERT INTO " +
                      $"{SchemaConstants.TableStore} " +
                      $"(" +
                      $"{SchemaConstants.AccountId}, " +
                      $"{SchemaConstants.StoreId}, " +
                      $"{SchemaConstants.Name}, " +
                      $"{SchemaConstants.Theme}, " +
                      $"{SchemaConstants.Subdomain}, " +
                      $"{SchemaConstants.Domain}" +
                      $") " +
                      $"VALUES " +
                      $"(" +
                      $"@accountId, " +
                      $"@storeId, " +
                      $"@name," +
                      $"@theme," +
                      $"@subdomain," +
                      $"@domain" +
                      $")";
            await db.ExecuteAsync(sql, new
            {
                accountId, store.StoreId, store.Name, store.Theme, store.Subdomain, store.Domain
            });
        }

        public async Task UpdateAsync(Store store)
        {
            var accountId = _context.GetAccountId();
            using var db = _db.GetConnection();
            var sql = $"UPDATE {SchemaConstants.TableStore} " +
                      $"SET " +
                      $"{SchemaConstants.Name} = @name, " +
                      $"{SchemaConstants.Theme} = @theme, " +
                      $"{SchemaConstants.Subdomain} = @subdomain, " +
                      $"{SchemaConstants.Domain} = @domain " +
                      $"WHERE {SchemaConstants.AccountId} = @accountId AND {SchemaConstants.StoreId} = @storeId";
            await db.ExecuteAsync(sql, new
            {
                accountId, store.StoreId, store.Name, store.Theme, store.Subdomain, store.Domain
            });
        }

        public async Task DeleteAsync(Store store)
        {
            var storeId = store.StoreId;
            var accountId = _context.GetAccountId();
            using var db = _db.GetConnection();
            var sql = $"DELETE FROM {SchemaConstants.TableStore} WHERE {SchemaConstants.AccountId} = @accountId AND {SchemaConstants.StoreId} = @storeId";
            await db.ExecuteAsync(sql, new {accountId, storeId});
        }
    }
}