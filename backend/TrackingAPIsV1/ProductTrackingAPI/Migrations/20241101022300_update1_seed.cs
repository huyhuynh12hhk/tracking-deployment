using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProductTrackingAPI.Migrations
{
    /// <inheritdoc />
    public partial class update1_seed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Key",
                table: "UserClaims");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "UserClaims");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "UserClaims");

            migrationBuilder.InsertData(
                table: "Claims",
                columns: new[] { "Id", "Key", "Provider", "Value" },
                values: new object[,]
                {
                    { "56676989-27ba-496e-8497-f213be2d81aa", "role", "None", "Admin" },
                    { "c46a98cd-59ea-4ad7-8460-4be27ad30532", "role", "None", "Supporter" },
                    { "f15c9a1a-1d9f-4c30-88ef-eddcb50d7b71", "role", "None", "Supplier" },
                    { "ffca2416-0be6-458c-8400-8954a1f07738", "role", "None", "Member" }
                });

            migrationBuilder.InsertData(
                table: "DetailUsers",
                columns: new[] { "Id", "Address", "AvatarImage", "BackgroundImage", "Description", "Email", "FullName", "Gender", "PhoneNumber" },
                values: new object[] { "7463ec1d-c26c-4a6e-85a0-55977782736d", "", "", "", "This is root admin account of this system", "admin01@gmail.com", "Admin System 01", "Not set", "" });

            migrationBuilder.InsertData(
                table: "UserAccounts",
                columns: new[] { "Id", "AccountType", "IsConfirmed", "Key", "Password", "Provider", "UserId" },
                values: new object[] { "9d50d3b9-d814-4deb-b7a3-fcf86f596714", "Admin", true, "admin01@gmail.com", "wyc5s2torGuiJts1zoF/O36V6KN9XvE1+0qhY8W5GC0=", "None", "7463ec1d-c26c-4a6e-85a0-55977782736d" });

            migrationBuilder.InsertData(
                table: "UserClaims",
                columns: new[] { "ClaimId", "UserId" },
                values: new object[] { "56676989-27ba-496e-8497-f213be2d81aa", "7463ec1d-c26c-4a6e-85a0-55977782736d" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "Id",
                keyValue: "c46a98cd-59ea-4ad7-8460-4be27ad30532");

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "Id",
                keyValue: "f15c9a1a-1d9f-4c30-88ef-eddcb50d7b71");

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "Id",
                keyValue: "ffca2416-0be6-458c-8400-8954a1f07738");

            migrationBuilder.DeleteData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: "9d50d3b9-d814-4deb-b7a3-fcf86f596714");

            migrationBuilder.DeleteData(
                table: "UserClaims",
                keyColumns: new[] { "ClaimId", "UserId" },
                keyValues: new object[] { "56676989-27ba-496e-8497-f213be2d81aa", "7463ec1d-c26c-4a6e-85a0-55977782736d" });

            migrationBuilder.DeleteData(
                table: "Claims",
                keyColumn: "Id",
                keyValue: "56676989-27ba-496e-8497-f213be2d81aa");

            migrationBuilder.DeleteData(
                table: "DetailUsers",
                keyColumn: "Id",
                keyValue: "7463ec1d-c26c-4a6e-85a0-55977782736d");

            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "UserClaims",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "UserClaims",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "UserClaims",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
