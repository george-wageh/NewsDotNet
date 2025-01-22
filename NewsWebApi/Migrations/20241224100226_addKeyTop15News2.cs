using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsWebApi.Migrations
{
    /// <inheritdoc />
    public partial class addKeyTop15News2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "Top15NewsModel");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Top15NewsModel",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
