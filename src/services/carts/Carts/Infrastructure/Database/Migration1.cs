using FluentMigrator;

namespace Carts.Infrastructure.Database
{
    [Migration(1)]
    public class Migration1 : Migration
    {
        public override void Up()
        {
            Create.Table(Constants.TableName)
                .WithColumn(Constants.CartIdColumn).AsString(36).NotNullable().PrimaryKey()
                .WithColumn(Constants.AccountIdColumn).AsString(36).NotNullable().PrimaryKey()
                .WithColumn(Constants.JsonColumn).AsCustom("jsonb").NotNullable();
        }

        public override void Down()
        {
            Delete.Table(Constants.TableName);
        }
    }
}