using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    public partial class UpdatePerson_StoredProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_UpdatePerson = @"CREATE PROCEDURE [dbo].[UpdatePerson](@PersonId uniqueidentifier,
                @PersonName nvarchar(40) , @Email nvarchar(40), @DateOfBirth datetime2(7),
                @Gender nvarchar(10), @CountryId uniqueIdentifier, @Address nvarchar(200),
                @ReceiveNewsLetter bit)
                AS BEGIN
                UPDATE [dbo].[Persons]
                SET PersonName = @PersonName, Email = @Email,
                DateOfBirth = @DateOfBirth,
                Gender = @Gender,
                CountryId = @CountryId,
                Address = @Address,
                ReceiveNewsLetter = @ReceiveNewsLetter
                WHERE PersonId = @PersonId
                END";
            migrationBuilder.Sql(sp_UpdatePerson);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sp_UpdatePerson = @"DROP PROCEDURE [dbo].[UpdatePerson]";
            migrationBuilder.Sql(sp_UpdatePerson);
        }
    }
}
