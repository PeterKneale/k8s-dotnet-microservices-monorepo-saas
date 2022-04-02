using System.Collections.Generic;
using System.Threading.Tasks;
using BuildingBlocks.Infrastructure.AccountContext;
using Catalog.Domain;
using Dapper;

namespace Catalog.Infrastructure.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly IDbConnectionFactory _db;
        private readonly IAccountContextGetter _context;

        public ProductRepository(IDbConnectionFactory db, IAccountContextGetter context)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;

            _db = db;
            _context = context;
        }

        public async Task<IEnumerable<Product>> ListAsync(string? categoryId = null)
        {
            var accountId = _context.GetAccountId();
            using var db = _db.GetConnection();
            if (categoryId == null)
            {
                var sql = $"SELECT * FROM {Constants.TableProduct} WHERE {Constants.ColumnAccountId} = @accountId";
                return await db.QueryAsync<Product>(sql, new {accountId});
            }
            else
            {
                var sql = $"SELECT * FROM {Constants.TableProduct} WHERE {Constants.ColumnAccountId} = @accountId AND {Constants.ColumnCategoryId} = @categoryId";
                return await db.QueryAsync<Product>(sql, new {accountId, categoryId});
            }
        }

        public async Task<Product?> GetByIdAsync(string productId)
        {
            var accountId = _context.GetAccountId();
            using var db = _db.GetConnection();
            var sql = $"SELECT * FROM {Constants.TableProduct} WHERE {Constants.ColumnAccountId} = @accountId AND {Constants.ColumnProductId} = @productId";
            return await db.QuerySingleOrDefaultAsync<Product>(sql, new {accountId, productId});
        }

        public async Task SaveAsync(Product product)
        {
            var accountId = _context.GetAccountId();
            using var db = _db.GetConnection();
            var sql = $"INSERT INTO " +
                      $"{Constants.TableProduct} " +
                      $"(" +
                          $"{Constants.ColumnAccountId}, " +
                          $"{Constants.ColumnProductId}, " +
                          $"{Constants.ColumnCategoryId}, " +
                          $"{Constants.ColumnName}, " +
                          $"{Constants.ColumnDescription}, " +
                          $"{Constants.ColumnPrice}, " +
                          $"{Constants.ColumnCurrencyCode}" +
                      $") " +
                      $"VALUES " +
                      $"(" +
                          $"@accountId, " +
                          $"@productId, " +
                          $"@categoryId, " +
                          $"@name, " +
                          $"@description, " +
                          $"@price, " +
                          $"@currencyCode" +
                      $")";
            await db.ExecuteAsync(sql, new {accountId, product.ProductId, product.CategoryId, product.Name, product.Description, product.Price, product.CurrencyCode});
        }

        public async Task UpdateAsync(Product product)
        {
            var accountId = _context.GetAccountId();
            using var db = _db.GetConnection();
            var sql = $"UPDATE {Constants.TableProduct} " +
                      $"SET " +
                      $"{Constants.ColumnCategoryId} = @categoryId, " +
                      $"{Constants.ColumnName} = @name, " +
                      $"{Constants.ColumnDescription} = @description, " +
                      $"{Constants.ColumnPrice} = @price, " +
                      $"{Constants.ColumnCurrencyCode} = @currencyCode " +
                      $"WHERE {Constants.ColumnAccountId} = @accountId AND {Constants.ColumnProductId} = @productId"; 
            await db.ExecuteAsync(sql, new {accountId, product.ProductId, product.CategoryId, product.Name, product.Description, product.Price, product.CurrencyCode});
        }

        public async Task DeleteAsync(Product product)
        {
            var productId = product.ProductId;
            var accountId = _context.GetAccountId();
            using var db = _db.GetConnection();
            var sql = $"DELETE FROM {Constants.TableProduct} WHERE {Constants.ColumnAccountId} = @accountId AND {Constants.ColumnProductId} = @productId";
            await db.ExecuteAsync(sql, new {accountId, productId});
        }
    }
}