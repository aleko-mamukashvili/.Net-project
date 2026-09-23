using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhysicalPersonsApp.Migrations
{
    /// <inheritdoc />
    public partial class test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhoneNumbers_Persons_PersonId",
                table: "PhoneNumbers");

            migrationBuilder.DropForeignKey(
                name: "FK_RelatedPersons_Persons_PersonId",
                table: "RelatedPersons");

            migrationBuilder.RenameColumn(
                name: "PersonId",
                table: "RelatedPersons",
                newName: "RelatedPersonPersonId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "RelatedPersons",
                newName: "RelatedPersonId");

            migrationBuilder.RenameIndex(
                name: "IX_RelatedPersons_PersonId",
                table: "RelatedPersons",
                newName: "IX_RelatedPersons_RelatedPersonPersonId");

            migrationBuilder.RenameColumn(
                name: "PersonId",
                table: "PhoneNumbers",
                newName: "PhonePersonId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "PhoneNumbers",
                newName: "PhoneId");

            migrationBuilder.RenameIndex(
                name: "IX_PhoneNumbers_PersonId",
                table: "PhoneNumbers",
                newName: "IX_PhoneNumbers_PhonePersonId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Persons",
                newName: "PersonId");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Cities",
                newName: "CityName");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Cities",
                newName: "CityId");

            migrationBuilder.RenameIndex(
                name: "IX_Cities_Name",
                table: "Cities",
                newName: "IX_Cities_CityName");

            migrationBuilder.AddForeignKey(
                name: "FK_PhoneNumbers_Persons_PhonePersonId",
                table: "PhoneNumbers",
                column: "PhonePersonId",
                principalTable: "Persons",
                principalColumn: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_RelatedPersons_Persons_RelatedPersonPersonId",
                table: "RelatedPersons",
                column: "RelatedPersonPersonId",
                principalTable: "Persons",
                principalColumn: "PersonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhoneNumbers_Persons_PhonePersonId",
                table: "PhoneNumbers");

            migrationBuilder.DropForeignKey(
                name: "FK_RelatedPersons_Persons_RelatedPersonPersonId",
                table: "RelatedPersons");

            migrationBuilder.RenameColumn(
                name: "RelatedPersonPersonId",
                table: "RelatedPersons",
                newName: "PersonId");

            migrationBuilder.RenameColumn(
                name: "RelatedPersonId",
                table: "RelatedPersons",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_RelatedPersons_RelatedPersonPersonId",
                table: "RelatedPersons",
                newName: "IX_RelatedPersons_PersonId");

            migrationBuilder.RenameColumn(
                name: "PhonePersonId",
                table: "PhoneNumbers",
                newName: "PersonId");

            migrationBuilder.RenameColumn(
                name: "PhoneId",
                table: "PhoneNumbers",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_PhoneNumbers_PhonePersonId",
                table: "PhoneNumbers",
                newName: "IX_PhoneNumbers_PersonId");

            migrationBuilder.RenameColumn(
                name: "PersonId",
                table: "Persons",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "CityName",
                table: "Cities",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "CityId",
                table: "Cities",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Cities_CityName",
                table: "Cities",
                newName: "IX_Cities_Name");

            migrationBuilder.AddForeignKey(
                name: "FK_PhoneNumbers_Persons_PersonId",
                table: "PhoneNumbers",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RelatedPersons_Persons_PersonId",
                table: "RelatedPersons",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id");
        }
    }
}
