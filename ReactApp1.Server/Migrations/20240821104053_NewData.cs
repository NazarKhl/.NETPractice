using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReactApp1.Server.Migrations
{
    /// <inheritdoc />
    public partial class NewData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Customers_CustomerId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Interventions_Addresses_AddressId",
                table: "Interventions");

            migrationBuilder.DropForeignKey(
                name: "FK_Interventions_Customers_CustomerId",
                table: "Interventions");

            migrationBuilder.DropForeignKey(
                name: "FK_Interventions_Users_UserId",
                table: "Interventions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Interventions",
                table: "Interventions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Addresses",
                table: "Addresses");

            migrationBuilder.RenameTable(
                name: "Interventions",
                newName: "Intervention");

            migrationBuilder.RenameTable(
                name: "Addresses",
                newName: "Address");

            migrationBuilder.RenameIndex(
                name: "IX_Interventions_UserId",
                table: "Intervention",
                newName: "IX_Intervention_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Interventions_CustomerId",
                table: "Intervention",
                newName: "IX_Intervention_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Interventions_AddressId",
                table: "Intervention",
                newName: "IX_Intervention_AddressId");

            migrationBuilder.RenameIndex(
                name: "IX_Addresses_CustomerId",
                table: "Address",
                newName: "IX_Address_CustomerId");

            migrationBuilder.AlterColumn<int>(
                name: "AddressId",
                table: "Intervention",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Intervention",
                table: "Intervention",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Address",
                table: "Address",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Address_Customers_CustomerId",
                table: "Address",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Intervention_Address_AddressId",
                table: "Intervention",
                column: "AddressId",
                principalTable: "Address",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Intervention_Customers_CustomerId",
                table: "Intervention",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Intervention_Users_UserId",
                table: "Intervention",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_Customers_CustomerId",
                table: "Address");

            migrationBuilder.DropForeignKey(
                name: "FK_Intervention_Address_AddressId",
                table: "Intervention");

            migrationBuilder.DropForeignKey(
                name: "FK_Intervention_Customers_CustomerId",
                table: "Intervention");

            migrationBuilder.DropForeignKey(
                name: "FK_Intervention_Users_UserId",
                table: "Intervention");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Intervention",
                table: "Intervention");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Address",
                table: "Address");

            migrationBuilder.RenameTable(
                name: "Intervention",
                newName: "Interventions");

            migrationBuilder.RenameTable(
                name: "Address",
                newName: "Addresses");

            migrationBuilder.RenameIndex(
                name: "IX_Intervention_UserId",
                table: "Interventions",
                newName: "IX_Interventions_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Intervention_CustomerId",
                table: "Interventions",
                newName: "IX_Interventions_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Intervention_AddressId",
                table: "Interventions",
                newName: "IX_Interventions_AddressId");

            migrationBuilder.RenameIndex(
                name: "IX_Address_CustomerId",
                table: "Addresses",
                newName: "IX_Addresses_CustomerId");

            migrationBuilder.AlterColumn<int>(
                name: "AddressId",
                table: "Interventions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Interventions",
                table: "Interventions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Addresses",
                table: "Addresses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Customers_CustomerId",
                table: "Addresses",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Interventions_Addresses_AddressId",
                table: "Interventions",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Interventions_Customers_CustomerId",
                table: "Interventions",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Interventions_Users_UserId",
                table: "Interventions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
