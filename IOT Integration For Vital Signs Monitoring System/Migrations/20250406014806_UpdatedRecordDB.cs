using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IOT_Integration_For_Vital_Signs_Monitoring_System.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedRecordDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Record",
                newName: "RecordId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RecordId",
                table: "Record",
                newName: "Id");
        }
    }
}
