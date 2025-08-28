using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JSMS.Migrations
{
    /// <inheritdoc />
    public partial class sujal2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "remark",
                table: "Apply",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Apply",
                keyColumn: "ApplyId",
                keyValue: 1,
                column: "remark",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "remark",
                table: "Apply");
        }
    }
}
