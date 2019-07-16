using Microsoft.EntityFrameworkCore.Migrations;

namespace DataContext.Migrations
{
    public partial class ValObjNameChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PersonName_Prefix",
                table: "ValObjHolders",
                newName: "SimplePersonName_Prefix");

            migrationBuilder.RenameColumn(
                name: "PersonName_Middle",
                table: "ValObjHolders",
                newName: "SimplePersonName_Middle");

            migrationBuilder.RenameColumn(
                name: "PersonName_Last",
                table: "ValObjHolders",
                newName: "SimplePersonName_Last");

            migrationBuilder.RenameColumn(
                name: "PersonName_First",
                table: "ValObjHolders",
                newName: "SimplePersonName_First");

            migrationBuilder.RenameColumn(
                name: "Address_Zip",
                table: "ValObjHolders",
                newName: "SimpleAddress_Zip");

            migrationBuilder.RenameColumn(
                name: "Address_State",
                table: "ValObjHolders",
                newName: "SimpleAddress_State");

            migrationBuilder.RenameColumn(
                name: "Address_Line2",
                table: "ValObjHolders",
                newName: "SimpleAddress_Line2");

            migrationBuilder.RenameColumn(
                name: "Address_Line1",
                table: "ValObjHolders",
                newName: "SimpleAddress_Line1");

            migrationBuilder.RenameColumn(
                name: "Address_City",
                table: "ValObjHolders",
                newName: "SimpleAddress_City");

            migrationBuilder.RenameColumn(
                name: "PersonName_Prefix",
                table: "Foos",
                newName: "SimplePersonName_Prefix");

            migrationBuilder.RenameColumn(
                name: "PersonName_Middle",
                table: "Foos",
                newName: "SimplePersonName_Middle");

            migrationBuilder.RenameColumn(
                name: "PersonName_Last",
                table: "Foos",
                newName: "SimplePersonName_Last");

            migrationBuilder.RenameColumn(
                name: "PersonName_First",
                table: "Foos",
                newName: "SimplePersonName_First");

            migrationBuilder.RenameColumn(
                name: "Address_Zip",
                table: "Foos",
                newName: "SimpleAddress_Zip");

            migrationBuilder.RenameColumn(
                name: "Address_State",
                table: "Foos",
                newName: "SimpleAddress_State");

            migrationBuilder.RenameColumn(
                name: "Address_Line2",
                table: "Foos",
                newName: "SimpleAddress_Line2");

            migrationBuilder.RenameColumn(
                name: "Address_Line1",
                table: "Foos",
                newName: "SimpleAddress_Line1");

            migrationBuilder.RenameColumn(
                name: "Address_City",
                table: "Foos",
                newName: "SimpleAddress_City");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SimplePersonName_Prefix",
                table: "ValObjHolders",
                newName: "PersonName_Prefix");

            migrationBuilder.RenameColumn(
                name: "SimplePersonName_Middle",
                table: "ValObjHolders",
                newName: "PersonName_Middle");

            migrationBuilder.RenameColumn(
                name: "SimplePersonName_Last",
                table: "ValObjHolders",
                newName: "PersonName_Last");

            migrationBuilder.RenameColumn(
                name: "SimplePersonName_First",
                table: "ValObjHolders",
                newName: "PersonName_First");

            migrationBuilder.RenameColumn(
                name: "SimpleAddress_Zip",
                table: "ValObjHolders",
                newName: "Address_Zip");

            migrationBuilder.RenameColumn(
                name: "SimpleAddress_State",
                table: "ValObjHolders",
                newName: "Address_State");

            migrationBuilder.RenameColumn(
                name: "SimpleAddress_Line2",
                table: "ValObjHolders",
                newName: "Address_Line2");

            migrationBuilder.RenameColumn(
                name: "SimpleAddress_Line1",
                table: "ValObjHolders",
                newName: "Address_Line1");

            migrationBuilder.RenameColumn(
                name: "SimpleAddress_City",
                table: "ValObjHolders",
                newName: "Address_City");

            migrationBuilder.RenameColumn(
                name: "SimplePersonName_Prefix",
                table: "Foos",
                newName: "PersonName_Prefix");

            migrationBuilder.RenameColumn(
                name: "SimplePersonName_Middle",
                table: "Foos",
                newName: "PersonName_Middle");

            migrationBuilder.RenameColumn(
                name: "SimplePersonName_Last",
                table: "Foos",
                newName: "PersonName_Last");

            migrationBuilder.RenameColumn(
                name: "SimplePersonName_First",
                table: "Foos",
                newName: "PersonName_First");

            migrationBuilder.RenameColumn(
                name: "SimpleAddress_Zip",
                table: "Foos",
                newName: "Address_Zip");

            migrationBuilder.RenameColumn(
                name: "SimpleAddress_State",
                table: "Foos",
                newName: "Address_State");

            migrationBuilder.RenameColumn(
                name: "SimpleAddress_Line2",
                table: "Foos",
                newName: "Address_Line2");

            migrationBuilder.RenameColumn(
                name: "SimpleAddress_Line1",
                table: "Foos",
                newName: "Address_Line1");

            migrationBuilder.RenameColumn(
                name: "SimpleAddress_City",
                table: "Foos",
                newName: "Address_City");
        }
    }
}
