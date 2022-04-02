using System.Collections.Generic;
using System.Threading.Tasks;
using BuildingBlocks.Infrastructure.AccountContext;
using Catalog.Domain;
using Dapper;

namespace Catalog.Infrastructure.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IDbConnectionFactory _db;
        private readonly IAccountContextGetter _context;

        public CategoryRepository(IDbConnectionFactory db, IAccountContextGetter context)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;

            _db = db;
            _context = context;
        }

        public async Task<IEnumerable<Category>> ListAsync()
        {
            var accountId = _context.GetAccountId();
            using var db = _db.GetConnection();
            var sql = $"SELECT * FROM {Constants.ViewCategory} WHERE {Constants.ColumnAccountId} = @accountId";
            return await db.QueryAsync<Category>(sql, new { accountId });
        }

        public async Task<Category?> GetByIdAsync(string categoryId)
        {
            var accountId = _context.GetAccountId();
            using var db = _db.GetConnection();
            var sql = $"SELECT * FROM {Constants.ViewCategory} WHERE {Constants.ColumnAccountId} = @accountId AND {Constants.ColumnCategoryId} = @categoryId";
            return await db.QuerySingleOrDefaultAsync<Category>(sql, new { accountId, categoryId });
        }

        public async Task SaveAsync(Category category)
        {
            var accountId = _context.GetAccountId();
            using var db = _db.GetConnection();
            var sql = $"INSERT INTO {Constants.TableCategory} " +
                      $"({Constants.ColumnAccountId}, {Constants.ColumnCategoryId}, {Constants.ColumnParentCategoryId}, {Constants.ColumnName}) VALUES" +
                      $" " +
                      $"(@accountId, @categoryId, @parentCategoryId, @name)";
            await db.ExecuteAsync(sql, new { accountId, category.CategoryId, category.ParentCategoryId, category.Name });
        }

        public async Task UpdateAsync(Category category)
        {
            var accountId = _context.GetAccountId();
            using var db = _db.GetConnection();
            var sql = $"UPDATE {Constants.TableCategory} " +
                      $"SET " +
                      $"{Constants.ColumnParentCategoryId} = @parentCategoryId, " +
                      $"{Constants.ColumnName} = @name " +
                      $"WHERE {Constants.ColumnAccountId} = @accountId AND {Constants.ColumnCategoryId} = @categoryId";
            await db.ExecuteAsync(sql, new { accountId, category.CategoryId, category.ParentCategoryId, category.Name });
        }

        public async Task DeleteAsync(Category category)
        {
            var categoryId = category.CategoryId;
            var accountId = _context.GetAccountId();
            using var db = _db.GetConnection();
            var sql = $"DELETE FROM {Constants.TableCategory} WHERE {Constants.ColumnAccountId} = @accountId AND {Constants.ColumnCategoryId} = @categoryId";
            await db.ExecuteAsync(sql, new { accountId, categoryId });
        }
    }
}