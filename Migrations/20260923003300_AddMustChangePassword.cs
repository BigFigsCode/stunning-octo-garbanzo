using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_1.Migrations
{
    /// <inheritdoc />
    public partial class AddMustChangePassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "userName",
                table: "Customers",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "password",
                table: "Customers",
                newName: "Password");

            migrationBuilder.AddColumn<bool>(
                name: "MustChangePassword",
                table: "Customers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MustChangePassword",
                table: "Customers");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Customers",
                newName: "userName");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Customers",
                newName: "password");
        }
    }
}
