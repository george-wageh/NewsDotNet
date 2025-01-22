using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsWebApi.Migrations
{
    /// <inheritdoc />
    public partial class addKeyTop15News : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "NewModel",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "NewModel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "ViewsCount",
                table: "NewModel",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "Top15NewsModel",
                columns: table => new
                {
                    NewModelId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    rank = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Top15NewsModel", x => x.NewModelId);
                    table.ForeignKey(
                        name: "FK_Top15NewsModel_NewModel_NewModelId",
                        column: x => x.NewModelId,
                        principalTable: "NewModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Top15NewsModel");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "NewModel");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "NewModel");

            migrationBuilder.DropColumn(
                name: "ViewsCount",
                table: "NewModel");
        }
    }
}
