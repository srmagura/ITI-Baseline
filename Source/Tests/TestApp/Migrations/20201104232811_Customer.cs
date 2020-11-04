using Microsoft.EntityFrameworkCore.Migrations;

namespace TestApp.Migrations
{
    public partial class Customer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SimpleAddress_Zip",
                table: "Customers",
                newName: "Address_Zip");

            migrationBuilder.RenameColumn(
                name: "SimpleAddress_State",
                table: "Customers",
                newName: "Address_State");

            migrationBuilder.RenameColumn(
                name: "SimpleAddress_Line2",
                table: "Customers",
                newName: "Address_Line2");

            migrationBuilder.RenameColumn(
                name: "SimpleAddress_Line1",
                table: "Customers",
                newName: "Address_Line1");

            migrationBuilder.RenameColumn(
                name: "SimpleAddress_HasValue",
                table: "Customers",
                newName: "Address_HasValue");

            migrationBuilder.RenameColumn(
                name: "SimpleAddress_City",
                table: "Customers",
                newName: "Address_City");

            migrationBuilder.RenameColumn(
                name: "SimplePersonName_Prefix",
                table: "Customers",
                newName: "ContactName_Prefix");

            migrationBuilder.RenameColumn(
                name: "SimplePersonName_Middle",
                table: "Customers",
                newName: "ContactName_Middle");

            migrationBuilder.RenameColumn(
                name: "SimplePersonName_Last",
                table: "Customers",
                newName: "ContactName_Last");

            migrationBuilder.RenameColumn(
                name: "SimplePersonName_HasValue",
                table: "Customers",
                newName: "ContactName_HasValue");

            migrationBuilder.RenameColumn(
                name: "SimplePersonName_First",
                table: "Customers",
                newName: "ContactName_First");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber_Value",
                table: "Customers",
                newName: "ContactPhone_Value");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber_HasValue",
                table: "Customers",
                newName: "ContactPhone_HasValue");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Address_Zip",
                table: "Customers",
                newName: "SimpleAddress_Zip");

            migrationBuilder.RenameColumn(
                name: "Address_State",
                table: "Customers",
                newName: "SimpleAddress_State");

            migrationBuilder.RenameColumn(
                name: "Address_Line2",
                table: "Customers",
                newName: "SimpleAddress_Line2");

            migrationBuilder.RenameColumn(
                name: "Address_Line1",
                table: "Customers",
                newName: "SimpleAddress_Line1");

            migrationBuilder.RenameColumn(
                name: "Address_HasValue",
                table: "Customers",
                newName: "SimpleAddress_HasValue");

            migrationBuilder.RenameColumn(
                name: "Address_City",
                table: "Customers",
                newName: "SimpleAddress_City");

            migrationBuilder.RenameColumn(
                name: "ContactName_Prefix",
                table: "Customers",
                newName: "SimplePersonName_Prefix");

            migrationBuilder.RenameColumn(
                name: "ContactName_Middle",
                table: "Customers",
                newName: "SimplePersonName_Middle");

            migrationBuilder.RenameColumn(
                name: "ContactName_Last",
                table: "Customers",
                newName: "SimplePersonName_Last");

            migrationBuilder.RenameColumn(
                name: "ContactName_HasValue",
                table: "Customers",
                newName: "SimplePersonName_HasValue");

            migrationBuilder.RenameColumn(
                name: "ContactName_First",
                table: "Customers",
                newName: "SimplePersonName_First");

            migrationBuilder.RenameColumn(
                name: "ContactPhone_Value",
                table: "Customers",
                newName: "PhoneNumber_Value");

            migrationBuilder.RenameColumn(
                name: "ContactPhone_HasValue",
                table: "Customers",
                newName: "PhoneNumber_HasValue");
        }
    }
}
