using Biblioteca.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;

namespace SistemaBiblioteca.API.Scripts;

public static class PromoverAdmin
{
    public static async Task ExecutarAsync(UserManager<Usuario> userManager, string email)
    {
        var usuario = await userManager.FindByEmailAsync(email);

        if(usuario is null)
        {
            Console.WriteLine("Usuário não encontrado. ");
            return;
        }

        var eAdmin = await userManager.IsInRoleAsync(usuario, "Admin");

        if (eAdmin)
        {
            Console.WriteLine("Usuário já é Admin");
        }
        else
        {
         var resultado = await userManager.AddToRoleAsync(usuario, "Admin");   
            if (resultado.Succeeded)
            {
            Console.WriteLine("Usuário promovido a Admin");
            }
            else
            {
                foreach(var erro in resultado.Errors)
                {
                Console.WriteLine(erro.Description);
                }
            }
        }
    }
}