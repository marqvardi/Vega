using Microsoft.EntityFrameworkCore.Migrations;

namespace Vega.Migrations
{
    public partial class SeedDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("insert into Makes (name) values ('Make1')");
            migrationBuilder.Sql("insert into Makes (name) values ('Make2')");
            migrationBuilder.Sql("insert into Makes (name) values ('Make3')");
            migrationBuilder.Sql("insert into Makes (name) values ('Make4')");

            migrationBuilder.Sql("insert into Models (name, makeId) values ('Make1-ModelA', 1)");
            migrationBuilder.Sql("insert into Models (name, makeId) values ('Make1-ModelB', 1)");
            migrationBuilder.Sql("insert into Models (name, makeId) values ('Make1-ModelC', 1)");

            migrationBuilder.Sql("insert into Models (name, makeId) values ('Make2-ModelA', 2)");
            migrationBuilder.Sql("insert into Models (name, makeId) values ('Make2-ModelB', 2)");
            migrationBuilder.Sql("insert into Models (name, makeId) values ('Make2-ModelC', 2)");

            migrationBuilder.Sql("insert into Models (name, makeId) values ('Make3-ModelA', 3)");
            migrationBuilder.Sql("insert into Models (name, makeId) values ('Make3-ModelB', 3)");
            migrationBuilder.Sql("insert into Models (name, makeId) values ('Make3-ModelC', 3)");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete from Makes");            
        }
    }
}
