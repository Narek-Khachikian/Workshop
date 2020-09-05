using Microsoft.EntityFrameworkCore.Migrations;

namespace WS.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Workshop");

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "Workshop",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(maxLength: 255, nullable: false),
                    Version = table.Column<string>(maxLength: 20, nullable: false),
                    Status = table.Column<bool>(nullable: true, defaultValueSql: "((1))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                schema: "Workshop",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierName = table.Column<string>(maxLength: 255, nullable: false),
                    Contact = table.Column<string>(maxLength: 50, nullable: false),
                    Address = table.Column<string>(maxLength: 255, nullable: true),
                    Status = table.Column<bool>(nullable: true, defaultValueSql: "((1))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Materials",
                schema: "Workshop",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaterialName = table.Column<string>(maxLength: 255, nullable: false),
                    Status = table.Column<bool>(nullable: true, defaultValueSql: "((1))"),
                    WSSuplierId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Materials_Suppliers_WSSuplierId",
                        column: x => x.WSSuplierId,
                        principalSchema: "Workshop",
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductMaterial",
                schema: "Workshop",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountInProduct = table.Column<double>(nullable: false),
                    CountUnit = table.Column<int>(nullable: false),
                    WSMaterialId = table.Column<long>(nullable: false),
                    WSProductId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductMaterial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductMaterial_Materials_WSMaterialId",
                        column: x => x.WSMaterialId,
                        principalSchema: "Workshop",
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductMaterial_Products_WSProductId",
                        column: x => x.WSProductId,
                        principalSchema: "Workshop",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Materials_WSSuplierId",
                schema: "Workshop",
                table: "Materials",
                column: "WSSuplierId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductMaterial_WSMaterialId",
                schema: "Workshop",
                table: "ProductMaterial",
                column: "WSMaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductMaterial_WSProductId",
                schema: "Workshop",
                table: "ProductMaterial",
                column: "WSProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductMaterial",
                schema: "Workshop");

            migrationBuilder.DropTable(
                name: "Materials",
                schema: "Workshop");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "Workshop");

            migrationBuilder.DropTable(
                name: "Suppliers",
                schema: "Workshop");
        }
    }
}
