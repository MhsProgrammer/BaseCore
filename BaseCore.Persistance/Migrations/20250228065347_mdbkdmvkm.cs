using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaseCore.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class mdbkdmvkm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PorudctName",
                table: "Products",
                newName: "ProductName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductName",
                table: "Products",
                newName: "PorudctName");
        }
    }
}
