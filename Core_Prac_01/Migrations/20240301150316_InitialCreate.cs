using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Core_Prac_01.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Applicants",
                columns: table => new
                {
                    ApplicantId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicantName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "date", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    AppliedFor = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Picture = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    IsReadyToWorkAnyWhere = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applicants", x => x.ApplicantId);
                });

            migrationBuilder.CreateTable(
                name: "Qualifications",
                columns: table => new
                {
                    QualificationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Degree = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Institute = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    PassingYear = table.Column<int>(type: "int", nullable: false),
                    Result = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ApplicantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Qualifications", x => x.QualificationId);
                    table.ForeignKey(
                        name: "FK_Qualifications_Applicants_ApplicantId",
                        column: x => x.ApplicantId,
                        principalTable: "Applicants",
                        principalColumn: "ApplicantId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Applicants",
                columns: new[] { "ApplicantId", "ApplicantName", "AppliedFor", "BirthDate", "Gender", "IsReadyToWorkAnyWhere", "Picture" },
                values: new object[] { 1, "A Applicant", "Peon", new DateTime(2005, 3, 1, 21, 3, 15, 377, DateTimeKind.Local).AddTicks(6892), 2, false, "1.jpg" });

            migrationBuilder.InsertData(
                table: "Qualifications",
                columns: new[] { "QualificationId", "ApplicantId", "Degree", "Institute", "PassingYear", "Result" },
                values: new object[,]
                {
                    { 1, 1, "SSC", "KPS", 2019, "4.5" },
                    { 2, 1, "HSC", "KPC", 2021, "5.0" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Qualifications_ApplicantId",
                table: "Qualifications",
                column: "ApplicantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Qualifications");

            migrationBuilder.DropTable(
                name: "Applicants");
        }
    }
}
