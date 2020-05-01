using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FlourMill_1.Migrations
{
    public partial class azureMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Administrator",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<byte[]>(nullable: true),
                    PasswordSalt = table.Column<byte[]>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    BirthDate = table.Column<string>(nullable: true),
                    NationalId = table.Column<long>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: true),
                    JobNumber = table.Column<string>(nullable: true),
                    TotalFlourMillPayment = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administrator", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bakery",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<byte[]>(nullable: true),
                    PasswordSalt = table.Column<byte[]>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    BirthDate = table.Column<string>(nullable: true),
                    NationalId = table.Column<long>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: true),
                    JobNumber = table.Column<string>(nullable: true),
                    address = table.Column<string>(nullable: true),
                    latitude = table.Column<double>(nullable: false),
                    longitude = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bakery", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SuperVisor",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<byte[]>(nullable: true),
                    PasswordSalt = table.Column<byte[]>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    BirthDate = table.Column<string>(nullable: true),
                    NationalId = table.Column<long>(nullable: false),
                    JobNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuperVisor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AdminRate",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<int>(nullable: false),
                    RateDate = table.Column<string>(nullable: true),
                    RateText = table.Column<string>(nullable: true),
                    AdministratorID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminRate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdminRate_Administrator_AdministratorID",
                        column: x => x.AdministratorID,
                        principalTable: "Administrator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    URL = table.Column<string>(nullable: true),
                    BadgeName = table.Column<string>(nullable: true),
                    BadgeType = table.Column<string>(nullable: true),
                    BadgeSize = table.Column<string>(nullable: true),
                    ProductionDate = table.Column<string>(nullable: true),
                    ExpireDate = table.Column<string>(nullable: true),
                    Usage = table.Column<string>(nullable: true),
                    ProductDescription = table.Column<string>(nullable: true),
                    price = table.Column<int>(nullable: false),
                    AdministratorID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Product_Administrator_AdministratorID",
                        column: x => x.AdministratorID,
                        principalTable: "Administrator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TruckDriver",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<byte[]>(nullable: true),
                    PasswordSalt = table.Column<byte[]>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    BirthDate = table.Column<string>(nullable: true),
                    NationalId = table.Column<long>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: true),
                    JobNumber = table.Column<string>(nullable: true),
                    AdministratorID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TruckDriver", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TruckDriver_Administrator_AdministratorID",
                        column: x => x.AdministratorID,
                        principalTable: "Administrator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Report",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Report_Date = table.Column<string>(nullable: true),
                    Flour_Mill_Name = table.Column<string>(nullable: true),
                    TotalPayment = table.Column<string>(nullable: true),
                    TotalBadgesForFlourMill = table.Column<double>(nullable: false),
                    AdministratorID = table.Column<int>(nullable: false),
                    SuperVisorID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Report", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Report_Administrator_AdministratorID",
                        column: x => x.AdministratorID,
                        principalTable: "Administrator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Report_SuperVisor_SuperVisorID",
                        column: x => x.SuperVisorID,
                        principalTable: "SuperVisor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductRate",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<int>(nullable: false),
                    RateDate = table.Column<string>(nullable: true),
                    RateText = table.Column<string>(nullable: true),
                    ProductID = table.Column<int>(nullable: false),
                    AdministratorID = table.Column<int>(nullable: false),
                    BakeryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductRate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductRate_Administrator_AdministratorID",
                        column: x => x.AdministratorID,
                        principalTable: "Administrator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductRate_Bakery_BakeryId",
                        column: x => x.BakeryId,
                        principalTable: "Bakery",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductRate_Product_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Product",
                        principalColumn: "ID"
                       );
                });

            migrationBuilder.CreateTable(
                name: "Wishlist",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    price = table.Column<int>(nullable: false),
                    Badgename = table.Column<string>(nullable: true),
                    url = table.Column<string>(nullable: true),
                    BakeryId = table.Column<int>(nullable: false),
                    AdministratorId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wishlist", x => x.id);
                    table.ForeignKey(
                        name: "FK_Wishlist_Administrator_AdministratorId",
                        column: x => x.AdministratorId,
                        principalTable: "Administrator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Wishlist_Bakery_BakeryId",
                        column: x => x.BakeryId,
                        principalTable: "Bakery",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Wishlist_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ID"
                       );
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                      ,
                    Order_Date = table.Column<string>(nullable: true),
                    TotalTons = table.Column<double>(nullable: false),
                    OrderStatues = table.Column<int>(nullable: false),
                    CustomerName = table.Column<string>(nullable: true),
                    Destination = table.Column<string>(nullable: true),
                    TotalPayment = table.Column<double>(nullable: false),
                    ShipmentPrice = table.Column<int>(nullable: false),
                    OrderComment = table.Column<string>(nullable: true),
                    AdministratorID = table.Column<int>(nullable: false),
                    BakeryID = table.Column<int>(nullable: false),
                    TruckDriverID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Order_Administrator_AdministratorID",
                        column: x => x.AdministratorID,
                        principalTable: "Administrator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_Bakery_BakeryID",
                        column: x => x.BakeryID,
                        principalTable: "Bakery",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_TruckDriver_TruckDriverID",
                        column: x => x.TruckDriverID,
                        principalTable: "TruckDriver",
                        principalColumn: "Id"
                       );
                });

            migrationBuilder.CreateTable(
                name: "orderProducts",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                     ,
                    Badge = table.Column<string>(nullable: true),
                    pic = table.Column<string>(nullable: true),
                    price = table.Column<int>(nullable: false),
                    tons = table.Column<int>(nullable: false),
                    orderId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orderProducts", x => x.id);
                    table.ForeignKey(
                        name: "FK_orderProducts_Order_orderId",
                        column: x => x.orderId,
                        principalTable: "Order",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminRate_AdministratorID",
                table: "AdminRate",
                column: "AdministratorID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_AdministratorID",
                table: "Order",
                column: "AdministratorID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_BakeryID",
                table: "Order",
                column: "BakeryID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_TruckDriverID",
                table: "Order",
                column: "TruckDriverID");

            migrationBuilder.CreateIndex(
                name: "IX_orderProducts_orderId",
                table: "orderProducts",
                column: "orderId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_AdministratorID",
                table: "Product",
                column: "AdministratorID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRate_AdministratorID",
                table: "ProductRate",
                column: "AdministratorID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRate_BakeryId",
                table: "ProductRate",
                column: "BakeryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRate_ProductID",
                table: "ProductRate",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Report_AdministratorID",
                table: "Report",
                column: "AdministratorID");

            migrationBuilder.CreateIndex(
                name: "IX_Report_SuperVisorID",
                table: "Report",
                column: "SuperVisorID");

            migrationBuilder.CreateIndex(
                name: "IX_TruckDriver_AdministratorID",
                table: "TruckDriver",
                column: "AdministratorID");

            migrationBuilder.CreateIndex(
                name: "IX_Wishlist_AdministratorId",
                table: "Wishlist",
                column: "AdministratorId");

            migrationBuilder.CreateIndex(
                name: "IX_Wishlist_BakeryId",
                table: "Wishlist",
                column: "BakeryId");

            migrationBuilder.CreateIndex(
                name: "IX_Wishlist_ProductId",
                table: "Wishlist",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminRate");

            migrationBuilder.DropTable(
                name: "orderProducts");

            migrationBuilder.DropTable(
                name: "ProductRate");

            migrationBuilder.DropTable(
                name: "Report");

            migrationBuilder.DropTable(
                name: "Wishlist");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "SuperVisor");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Bakery");

            migrationBuilder.DropTable(
                name: "TruckDriver");

            migrationBuilder.DropTable(
                name: "Administrator");
        }
    }
}
