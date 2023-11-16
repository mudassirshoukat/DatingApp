using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class UserAndPhotoEntity2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "id",
                keyValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "id", "City", "Country", "Created", "DateOfBirth", "Gender", "Interests", "Introduction", "KnownAs", "LastActive", "LookingFor", "PasswordHash", "PasswordSalt", "UserName" },
                values: new object[] { 1, "Sample City", "Sample Country", new DateTime(2023, 11, 15, 7, 39, 59, 573, DateTimeKind.Utc).AddTicks(3947), new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Male", "Programming, Reading", "Hello, I'm John Doe.", "John Doe", new DateTime(2023, 11, 15, 7, 39, 59, 573, DateTimeKind.Utc).AddTicks(3946), "Looking for someone special.", new byte[] { 1, 2, 3, 4 }, new byte[] { 5, 6, 7, 8 }, "SampleUser" });
        }
    }
}
