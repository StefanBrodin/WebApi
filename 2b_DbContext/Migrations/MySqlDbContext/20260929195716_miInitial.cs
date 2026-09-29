using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbContext.Migrations.MySqlDbContext
{
    /// <inheritdoc />
    public partial class miInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "supusr");

            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Category",
                schema: "supusr",
                columns: table => new
                {
                    CategoryId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CategoryName = table.Column<string>(type: "varchar(200)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Seeded = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.CategoryId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Country",
                schema: "supusr",
                columns: table => new
                {
                    CountryId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CountryName = table.Column<string>(type: "varchar(200)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Seeded = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.CountryId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CreditCards",
                columns: table => new
                {
                    CreditCardId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Issuer = table.Column<int>(type: "int", nullable: false),
                    Number = table.Column<string>(type: "varchar(200)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExpirationYear = table.Column<string>(type: "varchar(200)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExpirationMonth = table.Column<string>(type: "varchar(200)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CardHolderName = table.Column<string>(type: "varchar(200)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Seeded = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditCards", x => x.CreditCardId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "City",
                schema: "supusr",
                columns: table => new
                {
                    CityId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CityName = table.Column<string>(type: "varchar(200)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CountryId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Seeded = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.CityId);
                    table.ForeignKey(
                        name: "FK_City_Country_CountryId",
                        column: x => x.CountryId,
                        principalSchema: "supusr",
                        principalTable: "Country",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PostalCode",
                schema: "supusr",
                columns: table => new
                {
                    PostalCodeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PostalCode = table.Column<string>(type: "varchar(200)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CityId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Seeded = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostalCode", x => x.PostalCodeId);
                    table.ForeignKey(
                        name: "FK_PostalCode_City_CityId",
                        column: x => x.CityId,
                        principalSchema: "supusr",
                        principalTable: "City",
                        principalColumn: "CityId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Address",
                schema: "supusr",
                columns: table => new
                {
                    AddressId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    StreetName = table.Column<string>(type: "varchar(200)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StreetNumber = table.Column<string>(type: "varchar(200)", maxLength: 10, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PostalCodeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Seeded = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.AddressId);
                    table.ForeignKey(
                        name: "FK_Address_PostalCode_PostalCodeId",
                        column: x => x.PostalCodeId,
                        principalSchema: "supusr",
                        principalTable: "PostalCode",
                        principalColumn: "PostalCodeId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Attraction",
                schema: "supusr",
                columns: table => new
                {
                    AttractionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AttractionName = table.Column<string>(type: "varchar(200)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AttractionDescription = table.Column<string>(type: "varchar(200)", maxLength: 4000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AddressId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Seeded = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attraction", x => x.AttractionId);
                    table.ForeignKey(
                        name: "FK_Attraction_Address_AddressId",
                        column: x => x.AddressId,
                        principalSchema: "supusr",
                        principalTable: "Address",
                        principalColumn: "AddressId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Customer",
                schema: "supusr",
                columns: table => new
                {
                    CustomerId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CustomerFirstName = table.Column<string>(type: "varchar(200)", maxLength: 30, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CustomerLastName = table.Column<string>(type: "varchar(200)", maxLength: 30, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CustomerUserName = table.Column<string>(type: "varchar(200)", maxLength: 30, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AddressId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Seeded = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.CustomerId);
                    table.ForeignKey(
                        name: "FK_Customer_Address_AddressId",
                        column: x => x.AddressId,
                        principalSchema: "supusr",
                        principalTable: "Address",
                        principalColumn: "AddressId",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AttractionCategory",
                schema: "supusr",
                columns: table => new
                {
                    AttractionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CategoryId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Seeded = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttractionCategory", x => new { x.AttractionId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_AttractionCategory_Attraction_AttractionId",
                        column: x => x.AttractionId,
                        principalSchema: "supusr",
                        principalTable: "Attraction",
                        principalColumn: "AttractionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttractionCategory_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "supusr",
                        principalTable: "Category",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CustomerAttractionRating",
                schema: "supusr",
                columns: table => new
                {
                    CustomerId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AttractionId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RatingScore = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    RatingReview = table.Column<string>(type: "varchar(200)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RatingTimestamp = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Seeded = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerAttractionRating", x => new { x.CustomerId, x.AttractionId });
                    table.ForeignKey(
                        name: "FK_CustomerAttractionRating_Attraction_AttractionId",
                        column: x => x.AttractionId,
                        principalSchema: "supusr",
                        principalTable: "Attraction",
                        principalColumn: "AttractionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerAttractionRating_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "supusr",
                        principalTable: "Customer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Address_PostalCodeId",
                schema: "supusr",
                table: "Address",
                column: "PostalCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Attraction_AddressId",
                schema: "supusr",
                table: "Attraction",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Attraction_AttractionName",
                schema: "supusr",
                table: "Attraction",
                column: "AttractionName");

            migrationBuilder.CreateIndex(
                name: "IX_AttractionCategory_AttractionId_CategoryId",
                schema: "supusr",
                table: "AttractionCategory",
                columns: new[] { "AttractionId", "CategoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_AttractionCategory_CategoryId_AttractionId",
                schema: "supusr",
                table: "AttractionCategory",
                columns: new[] { "CategoryId", "AttractionId" });

            migrationBuilder.CreateIndex(
                name: "IX_Category_CategoryName",
                schema: "supusr",
                table: "Category",
                column: "CategoryName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_City_CountryId_CityName",
                schema: "supusr",
                table: "City",
                columns: new[] { "CountryId", "CityName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Country_CountryName",
                schema: "supusr",
                table: "Country",
                column: "CountryName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customer_AddressId",
                schema: "supusr",
                table: "Customer",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_CustomerUserName",
                schema: "supusr",
                table: "Customer",
                column: "CustomerUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAttractionRating_AttractionId",
                schema: "supusr",
                table: "CustomerAttractionRating",
                column: "AttractionId");

            migrationBuilder.CreateIndex(
                name: "IX_PostalCode_CityId_PostalCode",
                schema: "supusr",
                table: "PostalCode",
                columns: new[] { "CityId", "PostalCode" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttractionCategory",
                schema: "supusr");

            migrationBuilder.DropTable(
                name: "CreditCards");

            migrationBuilder.DropTable(
                name: "CustomerAttractionRating",
                schema: "supusr");

            migrationBuilder.DropTable(
                name: "Category",
                schema: "supusr");

            migrationBuilder.DropTable(
                name: "Attraction",
                schema: "supusr");

            migrationBuilder.DropTable(
                name: "Customer",
                schema: "supusr");

            migrationBuilder.DropTable(
                name: "Address",
                schema: "supusr");

            migrationBuilder.DropTable(
                name: "PostalCode",
                schema: "supusr");

            migrationBuilder.DropTable(
                name: "City",
                schema: "supusr");

            migrationBuilder.DropTable(
                name: "Country",
                schema: "supusr");
        }
    }
}
