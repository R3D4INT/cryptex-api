using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptexApi.Migrations
{
    /// <inheritdoc />
    public partial class Add_Silent_Mode_Field : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSilent",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSilent",
                table: "Users");
        }
    }
}
