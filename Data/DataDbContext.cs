using Biblioteca.Models;

using Microsoft.EntityFrameworkCore;
namespace Biblioteca.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Livro> Livros {get; set;}

        public DbSet<Usuario> Usuario{get; set;}
        public DbSet<Emprestimo> Emprestimo{get;set;}

         protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=biblioteca.db");
        }
    }   
}