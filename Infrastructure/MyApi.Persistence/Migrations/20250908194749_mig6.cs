using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyApi.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductDetails",
                table: "ProductDetails");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ProductDetails");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ProductDetails");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ProductDetails");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "ProductDetails");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "ProductDetails");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "ProductDocuments",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductDetails",
                table: "ProductDetails",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductDetails",
                table: "ProductDetails");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "ProductDocuments");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "ProductDetails",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ProductDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ProductDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "ProductDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "ProductDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductDetails",
                table: "ProductDetails",
                column: "Id");
        }
    }
}
