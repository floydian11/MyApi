using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyApi.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class identitymig_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "TCKNHash",
                table: "AspNetUsers",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "TCKNSalt",
                table: "AspNetUsers",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TCKNHash",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TCKNSalt",
                table: "AspNetUsers");
        }
    }
}
