using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MentalHealth.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Symptoms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Depression = table.Column<float>(type: "real", nullable: false),
                    Anxiety = table.Column<float>(type: "real", nullable: false),
                    SleepIssues = table.Column<float>(type: "real", nullable: false),
                    Fatigue = table.Column<float>(type: "real", nullable: false),
                    ConcentrationIssues = table.Column<float>(type: "real", nullable: false),
                    Irritability = table.Column<float>(type: "real", nullable: false),
                    SocialWithdrawal = table.Column<float>(type: "real", nullable: false),
                    EmotionalChanges = table.Column<float>(type: "real", nullable: false),
                    PhysicalSymptoms = table.Column<float>(type: "real", nullable: false),
                    ThoughtPatterns = table.Column<float>(type: "real", nullable: false),
                    Paranoia = table.Column<float>(type: "real", nullable: false),
                    Hallucinations = table.Column<float>(type: "real", nullable: false),
                    SubstanceUse = table.Column<float>(type: "real", nullable: false),
                    TraumaHistory = table.Column<float>(type: "real", nullable: false),
                    Diagnosis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Symptoms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainingData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImportDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RecordCount = table.Column<int>(type: "int", nullable: false),
                    ValidationAccuracy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingData", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Symptoms_CreatedAt",
                table: "Symptoms",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingData_ImportDate",
                table: "TrainingData",
                column: "ImportDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Symptoms");

            migrationBuilder.DropTable(
                name: "TrainingData");
        }
    }
}
