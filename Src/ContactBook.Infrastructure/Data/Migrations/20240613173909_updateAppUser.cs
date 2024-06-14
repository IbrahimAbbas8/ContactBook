using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContactBook.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateAppUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Profiles_ProfileId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "ProfileId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsInvite",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Profiles_ProfileId",
                table: "AspNetUsers",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Profiles_ProfileId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsInvite",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "ProfileId",
                table: "AspNetUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Profiles_ProfileId",
                table: "AspNetUsers",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id");
        }
    }
}
