using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SnipSmart.Migrations
{
    /// <inheritdoc />
    public partial class start_13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "f26afdcb-c079-4b3e-b342-59da160ede80", 0, "5d7a1617-6ddf-4e25-8be6-ba3e98a7fab2", "test@gmail.com", true, false, null, null, "TEST@GMAIL.COM", "AQAAAAIAAYagAAAAEBzFxLhj4J02RSNJTeFs8I92pRvJk+i5xvGI5z9W1al0D8savAM74IOAb0IjAofU6g==", null, false, "32c890db-047f-4fae-8ccc-ff33102484e6", false, "test@gmail.com" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f26afdcb-c079-4b3e-b342-59da160ede80");
        }
    }
}
