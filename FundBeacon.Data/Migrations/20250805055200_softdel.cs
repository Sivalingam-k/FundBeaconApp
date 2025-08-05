using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FundBeacon.Data.Migrations
{
    /// <inheritdoc />
    public partial class softdel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Scholarship",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Scholarship");
        }
    }
}
