using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    public partial class GetPersons_StoredProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_GetAllPersons =
                @"CREATE PROCEDURE [dbo].[GetAllPersons]
                as BEGIN 
                    SELECT PersonId, PersonName, Email,
                    DateOfBirth, Gender, CountryId,
                    Address, ReceiveNewsLetter
                    FROM [dbo].[Persons]
                END";
            migrationBuilder.Sql(sp_GetAllPersons);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sp_GetAllPersons = 
                @"DROP PROCEDURE [dbo].[GetAllPersons]";
            migrationBuilder.Sql(sp_GetAllPersons);
        }
    }
}
