using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    public partial class InsertPerson_SotredProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_InsertPerson = @"CREATE PROCEDURE [dbo].[InsertPerson] (@PersonId uniqueidentifier,
                @PersonName nvarchar(40) , @Email nvarchar(40), @DateOfBirth datetime2(7),
                @Gender nvarchar(10), @CountryId uniqueIdentifier, @Address nvarchar(200),
                @ReceiveNewsLetter bit)
                AS BEGIN 
                Insert Into [dbo].[Persons]
                (PersonId, PersonName, Email, DateOfBirth, Gender, CountryId, Address,
                ReceiveNewsLetter ) VALUES (@PersonId, @PersonName, @Email, @DateOfBirth,
                @Gender, @CountryId, @Address, @ReceiveNewsLetter)
                End";
            migrationBuilder.Sql(sp_InsertPerson);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sp_InsertPerson = @"DROP PROCEDURE [dbo].[InsertPerson]";
            migrationBuilder.Sql(sp_InsertPerson);

        }
    }
}
