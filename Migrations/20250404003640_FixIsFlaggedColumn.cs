using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MunicipalityManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class FixIsFlaggedColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<bool>(
            //    name: "IsFlagged",
            //    table: "Reports",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequests_CitizenID",
                table: "ServiceRequests",
                column: "CitizenID");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_CitizenID",
                table: "Reports",
                column: "CitizenID");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Citizens_CitizenID",
                table: "Reports",
                column: "CitizenID",
                principalTable: "Citizens",
                principalColumn: "CitizenID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRequests_Citizens_CitizenID",
                table: "ServiceRequests",
                column: "CitizenID",
                principalTable: "Citizens",
                principalColumn: "CitizenID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Citizens_CitizenID",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRequests_Citizens_CitizenID",
                table: "ServiceRequests");

            migrationBuilder.DropIndex(
                name: "IX_ServiceRequests_CitizenID",
                table: "ServiceRequests");

            migrationBuilder.DropIndex(
                name: "IX_Reports_CitizenID",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "IsFlagged",
                table: "Reports");
        }
    }
}
