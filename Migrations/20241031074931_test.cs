using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace blogApp.Migrations
{
    /// <inheritdoc />
    public partial class test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Posts_PostFId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_UserFId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Categories_CategoryFId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Users_UserFId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Comments_PostFId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_UserFId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "PostFId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "UserFId",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "UserFId",
                table: "Posts",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "CategoryFId",
                table: "Posts",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_UserFId",
                table: "Posts",
                newName: "IX_Posts_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_CategoryFId",
                table: "Posts",
                newName: "IX_Posts_CategoryId");

            migrationBuilder.AddColumn<int>(
                name: "PostId",
                table: "Comments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Comments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PostId",
                table: "Comments",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Posts_PostId",
                table: "Comments",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "PostId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Categories_CategoryId",
                table: "Posts",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Users_UserId",
                table: "Posts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Posts_PostId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Categories_CategoryId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Users_UserId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Comments_PostId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_UserId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Posts",
                newName: "UserFId");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Posts",
                newName: "CategoryFId");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_UserId",
                table: "Posts",
                newName: "IX_Posts_UserFId");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_CategoryId",
                table: "Posts",
                newName: "IX_Posts_CategoryFId");

            migrationBuilder.AddColumn<int>(
                name: "PostFId",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserFId",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PostFId",
                table: "Comments",
                column: "PostFId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserFId",
                table: "Comments",
                column: "UserFId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Posts_PostFId",
                table: "Comments",
                column: "PostFId",
                principalTable: "Posts",
                principalColumn: "PostId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_UserFId",
                table: "Comments",
                column: "UserFId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Categories_CategoryFId",
                table: "Posts",
                column: "CategoryFId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Users_UserFId",
                table: "Posts",
                column: "UserFId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
