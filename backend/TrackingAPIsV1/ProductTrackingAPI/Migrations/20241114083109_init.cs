using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProductTrackingAPI.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Claims",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Key = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false),
                    Provider = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Claims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DetailUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    FullName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    AvatarImage = table.Column<string>(type: "text", nullable: false),
                    BackgroundImage = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Address = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Gender = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Description = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetailUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserAccounts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IsConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    AccountType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Provider = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAccounts_DetailUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "DetailUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => new { x.UserId, x.ClaimId });
                    table.ForeignKey(
                        name: "FK_UserClaims_Claims_ClaimId",
                        column: x => x.ClaimId,
                        principalTable: "Claims",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserClaims_DetailUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "DetailUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Claims",
                columns: new[] { "Id", "Key", "Provider", "Value" },
                values: new object[,]
                {
                    { "26d75128-4fec-41bd-a3eb-cf618c037a0c", "role", "None", "Supplier" },
                    { "34e57829-da4c-4a05-ba45-2bc52796f1a9", "role", "None", "Supporter" },
                    { "62a11b73-7a9d-420a-afe0-732ec5db55c0", "role", "None", "Member" },
                    { "a917b969-b062-4d85-be3b-24137c5e268a", "role", "None", "Admin" }
                });

            migrationBuilder.InsertData(
                table: "DetailUsers",
                columns: new[] { "Id", "Address", "AvatarImage", "BackgroundImage", "Description", "Email", "FullName", "Gender", "PhoneNumber" },
                values: new object[] { "f7107f70-7648-4042-b583-a2a67ceaf580", "", "", "", "This is root admin account of this system", "admin01@gmail.com", "Admin System 01", "Not set", "" });

            migrationBuilder.InsertData(
                table: "UserAccounts",
                columns: new[] { "Id", "AccountType", "IsConfirmed", "Key", "Password", "Provider", "UserId" },
                values: new object[] { "e76f47bf-8e5d-4a98-a0cf-971d83b777e2", "Admin", true, "admin01@gmail.com", "wyc5s2torGuiJts1zoF/O36V6KN9XvE1+0qhY8W5GC0=", "None", "f7107f70-7648-4042-b583-a2a67ceaf580" });

            migrationBuilder.InsertData(
                table: "UserClaims",
                columns: new[] { "ClaimId", "UserId" },
                values: new object[] { "a917b969-b062-4d85-be3b-24137c5e268a", "f7107f70-7648-4042-b583-a2a67ceaf580" });

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_UserId",
                table: "UserAccounts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_ClaimId",
                table: "UserClaims",
                column: "ClaimId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAccounts");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "Claims");

            migrationBuilder.DropTable(
                name: "DetailUsers");
        }
    }
}
