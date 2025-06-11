using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_minimal.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Monitores",
                columns: table => new
                {
                    IdMonitor = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RA = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Apelido = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Monitores", x => x.IdMonitor);
                });

            migrationBuilder.CreateTable(
                name: "Horarios",
                columns: table => new
                {
                    IdHorario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiaSemana = table.Column<int>(type: "int", nullable: false),
                    HorarioAtendimento = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IdMonitor = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Horarios", x => x.IdHorario);
                    table.ForeignKey(
                        name: "FK_Horarios_Monitores_IdMonitor",
                        column: x => x.IdMonitor,
                        principalTable: "Monitores",
                        principalColumn: "IdMonitor",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Horarios_IdMonitor",
                table: "Horarios",
                column: "IdMonitor");

            migrationBuilder.CreateIndex(
                name: "IX_Monitores_Apelido",
                table: "Monitores",
                column: "Apelido",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Monitores_RA",
                table: "Monitores",
                column: "RA",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Horarios");

            migrationBuilder.DropTable(
                name: "Monitores");
        }
    }
}
