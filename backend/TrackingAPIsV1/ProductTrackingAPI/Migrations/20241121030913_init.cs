using System;
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
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    AuthorId = table.Column<string>(type: "text", nullable: false),
                    AttachmentString = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_DetailUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "DetailUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<double>(type: "double precision", nullable: false),
                    Discount = table.Column<double>(type: "double precision", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    GalleryString = table.Column<string>(type: "text", nullable: false),
                    SupplierId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_DetailUsers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "DetailUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Relationships",
                columns: table => new
                {
                    FromUserId = table.Column<string>(type: "text", nullable: false),
                    ToUserId = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Relationships", x => new { x.FromUserId, x.ToUserId });
                    table.ForeignKey(
                        name: "FK_Relationships_DetailUsers_FromUserId",
                        column: x => x.FromUserId,
                        principalTable: "DetailUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Relationships_DetailUsers_ToUserId",
                        column: x => x.ToUserId,
                        principalTable: "DetailUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateTable(
                name: "Reactions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    PostId = table.Column<string>(type: "text", nullable: false),
                    FromUserId = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reactions_DetailUsers_FromUserId",
                        column: x => x.FromUserId,
                        principalTable: "DetailUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reactions_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductOrigins",
                columns: table => new
                {
                    FromProductId = table.Column<string>(type: "text", nullable: false),
                    ToProductId = table.Column<string>(type: "text", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductOrigins", x => new { x.FromProductId, x.ToProductId });
                    table.ForeignKey(
                        name: "FK_ProductOrigins_Products_FromProductId",
                        column: x => x.FromProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductOrigins_Products_ToProductId",
                        column: x => x.ToProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Claims",
                columns: new[] { "Id", "Key", "Provider", "Value" },
                values: new object[,]
                {
                    { "983bb3f2-ef1f-4103-80b8-043baee77675", "role", "None", "Supporter" },
                    { "c97fa8fc-727e-4678-816c-6bd1d3b1e62d", "role", "None", "Supplier" },
                    { "ca72d269-20ae-4e4a-a517-5e73619a19f1", "role", "None", "Member" },
                    { "e4b0a209-181c-460c-9edc-eae8ff41d452", "role", "None", "Admin" }
                });

            migrationBuilder.InsertData(
                table: "DetailUsers",
                columns: new[] { "Id", "Address", "AvatarImage", "BackgroundImage", "Description", "Email", "FullName", "Gender", "PhoneNumber" },
                values: new object[] { "57e60e42-5363-479c-b5e5-61363772dc97", "", "", "", "This is root admin account of this system", "admin01@gmail.com", "Admin System 01", "Not set", "" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "Discount", "GalleryString", "Name", "Price", "SupplierId" },
                values: new object[] { "fdeafeda-c730-4740-9dd9-8fa68c89a670", "", 0.0, "", "Cake Coffe", 300.0, "57e60e42-5363-479c-b5e5-61363772dc97" });

            migrationBuilder.InsertData(
                table: "UserAccounts",
                columns: new[] { "Id", "AccountType", "IsConfirmed", "Key", "Password", "Provider", "UserId" },
                values: new object[] { "857c7ce7-16fb-4667-8188-25a0e390f275", "Admin", true, "admin01@gmail.com", "wyc5s2torGuiJts1zoF/O36V6KN9XvE1+0qhY8W5GC0=", "None", "57e60e42-5363-479c-b5e5-61363772dc97" });

            migrationBuilder.InsertData(
                table: "UserClaims",
                columns: new[] { "ClaimId", "UserId" },
                values: new object[] { "e4b0a209-181c-460c-9edc-eae8ff41d452", "57e60e42-5363-479c-b5e5-61363772dc97" });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_AuthorId",
                table: "Posts",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOrigins_ToProductId",
                table: "ProductOrigins",
                column: "ToProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SupplierId",
                table: "Products",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_FromUserId",
                table: "Reactions",
                column: "FromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_PostId",
                table: "Reactions",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Relationships_ToUserId",
                table: "Relationships",
                column: "ToUserId");

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
                name: "ProductOrigins");

            migrationBuilder.DropTable(
                name: "Reactions");

            migrationBuilder.DropTable(
                name: "Relationships");

            migrationBuilder.DropTable(
                name: "UserAccounts");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Claims");

            migrationBuilder.DropTable(
                name: "DetailUsers");
        }
    }
}
