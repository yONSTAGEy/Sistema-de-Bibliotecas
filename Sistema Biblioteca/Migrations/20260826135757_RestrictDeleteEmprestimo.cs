using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sistema_Biblioteca.Migrations
{
    /// <inheritdoc />
    public partial class RestrictDeleteEmprestimo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Emprestimo_Livros_LivroID",
                table: "Emprestimo");

            migrationBuilder.DropForeignKey(
                name: "FK_Emprestimo_Usuario_UsuarioID",
                table: "Emprestimo");

            migrationBuilder.AddForeignKey(
                name: "FK_Emprestimo_Livros_LivroID",
                table: "Emprestimo",
                column: "LivroID",
                principalTable: "Livros",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Emprestimo_Usuario_UsuarioID",
                table: "Emprestimo",
                column: "UsuarioID",
                principalTable: "Usuario",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Emprestimo_Livros_LivroID",
                table: "Emprestimo");

            migrationBuilder.DropForeignKey(
                name: "FK_Emprestimo_Usuario_UsuarioID",
                table: "Emprestimo");

            migrationBuilder.AddForeignKey(
                name: "FK_Emprestimo_Livros_LivroID",
                table: "Emprestimo",
                column: "LivroID",
                principalTable: "Livros",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Emprestimo_Usuario_UsuarioID",
                table: "Emprestimo",
                column: "UsuarioID",
                principalTable: "Usuario",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
