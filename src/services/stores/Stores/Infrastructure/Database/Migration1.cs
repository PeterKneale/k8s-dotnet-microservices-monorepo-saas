using FluentMigrator;
using Stores.Infrastructure.Repository;

namespace Stores.Infrastructure.Database
{
    [Migration(1)]
    public class Migration1 : Migration
    {
        public override void Up()
        {
            Create.Table(SchemaConstants.TableStore)
                .WithColumn(SchemaConstants.AccountId).AsString(36)
                .WithColumn(SchemaConstants.StoreId).AsString(36).PrimaryKey()
                .WithColumn(SchemaConstants.Name).AsString(100)
                .WithColumn(SchemaConstants.Theme).AsString(100)
                .WithColumn(SchemaConstants.Subdomain).AsString(1000)
                .WithColumn(SchemaConstants.Domain).AsString(1000).Nullable();
        }

        public override void Down()
        {
            Delete.Table(SchemaConstants.TableStore);
        }
    }
}