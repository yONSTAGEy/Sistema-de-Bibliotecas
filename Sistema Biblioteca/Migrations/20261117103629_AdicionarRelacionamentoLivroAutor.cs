using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sistema_Biblioteca.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarRelacionamentoLivroAutor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "Autores",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Autores", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AutorLivro",
                columns: table => new
                {
                    AutoresID = table.Column<int>(type: "INTEGER", nullable: false),
                    LivrosID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutorLivro", x => new { x.AutoresID, x.LivrosID });
                    table.ForeignKey(
                        name: "FK_AutorLivro_Autores_AutoresID",
                        column: x => x.AutoresID,
                        principalTable: "Autores",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AutorLivro_Livros_LivrosID",
                        column: x => x.LivrosID,
                        principalTable: "Livros",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AutorLivro_LivrosID",
                table: "AutorLivro",
                column: "LivrosID");

            migrationBuilder.Sql(@"
                INSERT INTO Autores (Nome)
                SELECT MIN(TRIM(Autor))
                FROM Livros
                WHERE Autor IS NOT NULL AND TRIM(Autor) <> ''
                GROUP BY TRIM(Autor) COLLATE NOCASE;
                ");

            migrationBuilder.Sql(@"
                INSERT INTO AutorLivro (AutoresID, LivrosID)
                SELECT a.ID, l.ID
                FROM Livros l
                INNER JOIN Autores a
                ON TRIM(l.Autor) = a.Nome COLLATE NOCASE
                WHERE l.Autor IS NOT NULL AND TRIM(l.Autor) <> '';
                ");

            migrationBuilder.DropColumn(
            name: "Autor",
            table: "Livros");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Autor",
                table: "Livros",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql(@"
            UPDATE Livros
            SET Autor = (
            SELECT GROUP_CONCAT(a.Nome, ', ')
            FROM AutorLivro al
            INNER JOIN Autores a ON a.ID = al.AutoresID
            WHERE al.LivrosID = Livros.ID
            )
            WHERE EXISTS (
            SELECT 1
            FROM AutorLivro al
            WHERE al.LivrosID = Livros.ID
                );
            ");

            migrationBuilder.DropTable(
                name: "AutorLivro");

            migrationBuilder.DropTable(
                name: "Autores");


        }
    }
}
