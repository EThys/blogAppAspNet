using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace blogApp.Migrations
{
    /// <inheritdoc />
    public partial class ClearAllTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Comments");
            migrationBuilder.Sql("DELETE FROM Posts");
            migrationBuilder.Sql("DELETE FROM Categories");
            migrationBuilder.Sql("DELETE FROM Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
