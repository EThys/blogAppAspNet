using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace blogApp.Migrations
{
    /// <inheritdoc />
    public partial class RefactorDateTimeForComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PostTitle",
                table: "Posts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Comments",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostTitle",
                table: "Posts");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "CreatedAt",
                table: "Comments",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
