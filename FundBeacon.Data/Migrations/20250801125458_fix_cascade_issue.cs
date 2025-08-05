using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FundBeacon.Data.Migrations
{
    /// <inheritdoc />
    public partial class fix_cascade_issue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScholarshipProviders",
                columns: table => new
                {
                    ProviderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentProviderId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScholarshipProviders", x => x.ProviderId);
                    table.ForeignKey(
                        name: "FK_ScholarshipProviders_ScholarshipProviders_ParentProviderId",
                        column: x => x.ParentProviderId,
                        principalTable: "ScholarshipProviders",
                        principalColumn: "ProviderId");
                });

            migrationBuilder.CreateTable(
                name: "Scholarship",
                columns: table => new
                {
                    ScholarshipId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    About = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Eligibility = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Deadline = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Benefits = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DocumentsRequired = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HowCanYouApply = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactUs = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Disclaimer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProviderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scholarship", x => x.ScholarshipId);
                    table.ForeignKey(
                        name: "FK_Scholarship_ScholarshipProviders_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "ScholarshipProviders",
                        principalColumn: "ProviderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScholarshipApplication",
                columns: table => new
                {
                    ApplicationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScholarshipId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    ApplicationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubmittedDocuments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VerificationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScholarshipApplication", x => x.ApplicationId);
                    table.ForeignKey(
                        name: "FK_ScholarshipApplication_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScholarshipApplication_Scholarship_ScholarshipId",
                        column: x => x.ScholarshipId,
                        principalTable: "Scholarship",
                        principalColumn: "ScholarshipId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScholarshipApplicationVerifications",
                columns: table => new
                {
                    VerificationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationId = table.Column<int>(type: "int", nullable: false),
                    SubProviderId = table.Column<int>(type: "int", nullable: false),
                    VerifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VerificationStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScholarshipApplicationVerifications", x => x.VerificationId);
                    table.ForeignKey(
                        name: "FK_ScholarshipApplicationVerifications_ScholarshipApplication_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "ScholarshipApplication",
                        principalColumn: "ApplicationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScholarshipApplicationVerifications_ScholarshipProviders_SubProviderId",
                        column: x => x.SubProviderId,
                        principalTable: "ScholarshipProviders",
                        principalColumn: "ProviderId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Scholarship_ProviderId",
                table: "Scholarship",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_ScholarshipApplication_CustomerId",
                table: "ScholarshipApplication",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ScholarshipApplication_ScholarshipId",
                table: "ScholarshipApplication",
                column: "ScholarshipId");

            migrationBuilder.CreateIndex(
                name: "IX_ScholarshipApplication_VerificationId",
                table: "ScholarshipApplication",
                column: "VerificationId");

            migrationBuilder.CreateIndex(
                name: "IX_ScholarshipApplicationVerifications_ApplicationId",
                table: "ScholarshipApplicationVerifications",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_ScholarshipApplicationVerifications_SubProviderId",
                table: "ScholarshipApplicationVerifications",
                column: "SubProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_ScholarshipProviders_ParentProviderId",
                table: "ScholarshipProviders",
                column: "ParentProviderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScholarshipApplication_ScholarshipApplicationVerifications_VerificationId",
                table: "ScholarshipApplication",
                column: "VerificationId",
                principalTable: "ScholarshipApplicationVerifications",
                principalColumn: "VerificationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Scholarship_ScholarshipProviders_ProviderId",
                table: "Scholarship");

            migrationBuilder.DropForeignKey(
                name: "FK_ScholarshipApplicationVerifications_ScholarshipProviders_SubProviderId",
                table: "ScholarshipApplicationVerifications");

            migrationBuilder.DropForeignKey(
                name: "FK_ScholarshipApplication_ScholarshipApplicationVerifications_VerificationId",
                table: "ScholarshipApplication");

            migrationBuilder.DropTable(
                name: "ScholarshipProviders");

            migrationBuilder.DropTable(
                name: "ScholarshipApplicationVerifications");

            migrationBuilder.DropTable(
                name: "ScholarshipApplication");

            migrationBuilder.DropTable(
                name: "Scholarship");
        }
    }
}
