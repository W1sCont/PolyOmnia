using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PolyOmnia.Migrations
{
    /// <inheritdoc />
    public partial class addThumbnailUrlOrPathToVideo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ThumbnailUrlOrPath",
                table: "Videos",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThumbnailUrlOrPath",
                table: "Videos");
        }
    }
}
