using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;

#nullable disable

namespace Entities.Migrations
{
    public partial class DeletePerson_StoredProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        { 
            string sp_DeletePerson = @"CREATE PROCEDURE [dbo].[DeletePerson] (@PersonId uniqueidentifier)
                AS BEGIN 
                DELETE FROM [dbo].[Persons]
                WHERE PersonId = @PersonId
                End";
            migrationBuilder.Sql(sp_DeletePerson);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sp_DeletePerson = @"DROP PROCEDURE [dbo].[DeletePerson]";
            migrationBuilder.Sql(sp_DeletePerson);
        }
    }
}
