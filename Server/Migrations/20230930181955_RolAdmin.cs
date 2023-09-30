using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorPeliculas.Server.Migrations
{
    /// <inheritdoc />
    public partial class RolAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                $"INSERT INTO AspNetRoles(Id, Name, NormalizedName) VALUES ('0a8f9cc1-b964-4b83-bf3d-9b7043428bbb', 'admin', 'ADMIN')"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "DELETE AspNetRoles WHERE Id = '0a8f9cc1-b964-4b83-bf3d-9b7043428bbb'"
            );
        }
    }
}