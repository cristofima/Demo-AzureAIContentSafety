using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AzureAIContentSafety.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TextRequiresModeration = table.Column<bool>(type: "bit", nullable: false),
                    TextIsHarmful = table.Column<bool>(type: "bit", nullable: false),
                    TextHateSeverity = table.Column<int>(type: "int", nullable: false),
                    TextSelfHarmSeverity = table.Column<int>(type: "int", nullable: false),
                    TextSexualSeverity = table.Column<int>(type: "int", nullable: false),
                    TextViolenceSeverity = table.Column<int>(type: "int", nullable: false),
                    ImageRequiresModeration = table.Column<bool>(type: "bit", nullable: false),
                    ImageIsHarmful = table.Column<bool>(type: "bit", nullable: false),
                    ImageHateSeverity = table.Column<int>(type: "int", nullable: false),
                    ImageSelfHarmSeverity = table.Column<int>(type: "int", nullable: false),
                    ImageSexualSeverity = table.Column<int>(type: "int", nullable: false),
                    ImageViolenceSeverity = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Posts");
        }
    }
}
