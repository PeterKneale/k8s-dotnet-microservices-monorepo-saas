using System.Data;
using System.Linq;
using FluentMigrator;

namespace Catalog.Infrastructure.Database
{
    [Migration(1)]
    public class Migration1 : Migration
    {
        public override void Up()
        {
            Create.Table(Constants.TableProduct)
                .WithColumn(Constants.ColumnAccountId).AsString(36)
                .WithColumn(Constants.ColumnProductId).AsString(36).PrimaryKey()
                .WithColumn(Constants.ColumnCategoryId).AsString(36)
                .WithColumn(Constants.ColumnName).AsString(100)
                .WithColumn(Constants.ColumnDescription).AsString(200).Nullable()
                .WithColumn(Constants.ColumnPrice).AsDecimal()
                .WithColumn(Constants.ColumnCurrencyCode).AsString(3);

            Create.Table(Constants.TableCategory)
                .WithColumn(Constants.ColumnAccountId).AsString(36)
                .WithColumn(Constants.ColumnCategoryId).AsString(36).PrimaryKey()
                .WithColumn(Constants.ColumnParentCategoryId).AsString(36).Nullable()
                .WithColumn(Constants.ColumnName).AsString(50);

            Create.ForeignKey("product_category")
                .FromTable(Constants.TableProduct).ForeignColumn(Constants.ColumnCategoryId)
                .ToTable(Constants.TableCategory).PrimaryColumn(Constants.ColumnCategoryId)
                .OnDelete(Rule.Cascade);
            
            Create.ForeignKey("category_parent_category")
                .FromTable(Constants.TableCategory).ForeignColumn(Constants.ColumnParentCategoryId)
                .ToTable(Constants.TableCategory).PrimaryColumn(Constants.ColumnCategoryId)
                .OnDelete(Rule.Cascade);
            
            var views = GetType().Assembly.GetManifestResourceNames().Where(x => x.Contains("_view.sql"));
            foreach (var view in views) Execute.EmbeddedScript(view);
        }

        public override void Down()
        {
            Execute.Sql("DROP VIEW IF EXISTS category_view;");
            Delete.Table(Constants.TableProduct);
            Delete.Table(Constants.TableCategory);
        }
    }
}