using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Biblioteca.Models;

using Microsoft.EntityFrameworkCore;
namespace Biblioteca.Data
{
    public class AppDbContext : IdentityDbContext<Usuario, IdentityRole<int>, int>
    {
        public DbSet<Livro> Livros { get; set; }
        public DbSet<Autor> Autores { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Emprestimo> Emprestimo { get; set; }

        public AppDbContext()
        {

        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                DirectoryInfo? diretorio = new DirectoryInfo(AppContext.BaseDirectory);

                while (diretorio != null)
                {
                    string caminhoProjeto = Path.Combine
                    (
                    diretorio.FullName,
                    "Sistema Biblioteca",
                    "Sistema Biblioteca.csproj"
                    );

                    if (File.Exists(caminhoProjeto))
                    {
                        string caminhoBanco = Path.Combine(
                            diretorio.FullName,
                            "Sistema Biblioteca",
                            "biblioteca.db"
                        );
                        Console.WriteLine($"Banco utilizado: {caminhoBanco}");

                        optionsBuilder.UseSqlite($"Data Source={caminhoBanco}");
                        return;
                    }

                    diretorio = diretorio.Parent;
                }

                throw new DirectoryNotFoundException
                (
                "Não foi possível localizar a pasta do projeto Sistema Biblioteca."
                );
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>().ToTable("Usuario");

            modelBuilder.Entity<Usuario>()
            .Property(u => u.Id)
            .HasColumnName("ID");

            modelBuilder.Entity<Emprestimo>()
        .HasOne(e => e.Livros)
        .WithMany()
        .HasForeignKey(e => e.LivroID)
        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Emprestimo>()
            .HasOne(e => e.Usuario)
            .WithMany()
            .HasForeignKey(e => e.UsuarioID)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Livro>()
            .HasMany(l => l.Autores)
            .WithMany(a => a.Livros);
        }
    }
}