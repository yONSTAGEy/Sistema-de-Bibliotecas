using Biblioteca.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Biblioteca.Models;
using System.Security.Claims;
using SistemaBiblioteca.API.DTOs;

namespace SistemaBiblioteca.API.Controllers;

[Route("api/[controller]")]
[ApiController]

public class EmprestimoController : ControllerBase
{
    private readonly EmprestimoService _emprestimoService;

    public EmprestimoController(EmprestimoService emprestimoService)
    {
        _emprestimoService = emprestimoService;
    }

    [HttpPost]
    [Authorize(Roles = "Cliente")]

    public IActionResult SolicitarEmprestimo(int livroID)
    {
        var usuarioID = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(usuarioID, out int usuarioId))
        {
            return Unauthorized();
        }

        var VerificarLivro = _emprestimoService.BuscarLivro(livroID);
        if (VerificarLivro == null)
        {
            return NotFound();
        }


        var emprestimo = new Emprestimo
        {
            LivroID = livroID,
            UsuarioID = usuarioId
        };

        var sucesso = _emprestimoService.Emprestimo(emprestimo);

        if (sucesso == "Empréstimo feito")
        {
            return Ok();
        }
        else if (sucesso == "Livro não encontrado")
        {
            return NotFound("Livro não encontrado");
        }
        else if (sucesso == "Usuário não encontrado")
        {
            return NotFound("Usuário não encontrado");
        }
        else if (sucesso == "Livro indisponível")
        {
            return Conflict("Livro indisponível");
        }
        else
        {
            return BadRequest("Erro");
        }
    }

    [HttpPut("Devolver/{emprestimoID}")]
    [Authorize(Roles = "Cliente")]
    public IActionResult DevolverEmprestimo(int emprestimoID)
    {
        var usuarioID = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(usuarioID, out int usuarioId))
        {
            return Unauthorized();
        }

        var Devolução = _emprestimoService.DevolverEmprestimo(emprestimoID, usuarioId);
        if (Devolução == "Empréstimo não encontrado")
        {
            return NotFound("Empréstimo não encontrado");
        }
        else if (Devolução == "Empréstimo não pertence ao usuário")
        {
            return NotFound("Empréstimo não pertence ao usuário");
        }
        else if (Devolução == "Livro já devolvido")
        {
            return BadRequest("Livro já devolvido");
        }
        else if (Devolução == "Devolução realizada")
        {
            return Ok("Devolução realizada");
        }
        else
        {
            return BadRequest("Erro");
        }
    }

    [HttpGet("Meus empréstimos")]
    [Authorize(Roles = "Cliente")]
    public IActionResult MeusEmprestimos()
    {
        var emprestimoID = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(emprestimoID, out int usuarioId))
        {
            return Unauthorized();
        }

        var Listar = _emprestimoService.ListarEmprestimosUsuario(usuarioId);
        return Ok(Listar);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult ListarEmprestimos()
    {
        var listar = _emprestimoService.ListarEmprestimo();
        return Ok(listar);
    }

    [HttpGet("Atrasados")]
    [Authorize(Roles = "Admin")]
    public IActionResult ListarEmprestimosAtrasados()
    {
        var ListarEmprestimosAtrasados = _emprestimoService.ListarEmprestimosAtrasadoAdmin();
        var TransformarEmDTO = ListarEmprestimosAtrasados.Select(e => new LivrosEmprestadosAtrasadosDTO
        {
            ID =  e.ID,
            LivroID =  e.LivroID,
            UsuarioID =  e.UsuarioID,
            DataDevolucaoPrevista =  e.DataDevolucaoPrevista,
            Multa = e.CalcularMulta()
        }).ToList();
        return Ok(TransformarEmDTO);
    }


}
