using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class PopulateDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Company Id", "Address", "Country", "Name" },
                values: new object[] { new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"), "Lagos", "Nigeria", "Capricorn Digital Limited" });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Company Id", "Address", "Country", "Name" },
                values: new object[] { new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3"), "Lagos", "Nigeria", "Appcoy Technologies" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Employee Id", "Age", "CompanyId", "Name", "Position" },
                values: new object[] { new Guid("80abbca8-664d-4b20-b5de-024705497d4a"), 28, new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"), "Adegoke Michael", "Tech Support Officer" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Employee Id", "Age", "CompanyId", "Name", "Position" },
                values: new object[] { new Guid("86dba8c0-d178-41e7-938c-ed49778fb52a"), 19, new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3"), "Tijani Hussein", "Software Developer" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Employee Id", "Age", "CompanyId", "Name", "Position" },
                values: new object[] { new Guid("021ca3c1-0deb-4afd-ae94-2159a8479811"), 36, new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3"), "Osinnowo Emmanuel", "CTO COO CEO" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Employee Id",
                keyValue: new Guid("021ca3c1-0deb-4afd-ae94-2159a8479811"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Employee Id",
                keyValue: new Guid("80abbca8-664d-4b20-b5de-024705497d4a"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Employee Id",
                keyValue: new Guid("86dba8c0-d178-41e7-938c-ed49778fb52a"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Company Id",
                keyValue: new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Company Id",
                keyValue: new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"));
        }
    }
}
