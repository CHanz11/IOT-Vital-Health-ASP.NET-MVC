using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IOT_Integration_For_Vital_Signs_Monitoring_System.Migrations
{
    /// <inheritdoc />
    public partial class AddedProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfileImagePath",
                table: "Patient",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileImagePath",
                table: "Patient");
        }
    }
}
