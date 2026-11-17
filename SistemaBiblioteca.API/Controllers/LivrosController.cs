using Biblioteca.Models;
using Biblioteca.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaDeBiblioteca.API.Services;

namespace SistemaBiblioteca.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LivrosController : ControllerBase
    {
        private readonly GoogleBooksService _googleBooksService;
        private readonly LivroService _livroService;

        public LivrosController(LivroService livroService, GoogleBooksService googleBooksService)
        {
            _googleBooksService = googleBooksService;
            _livroService = livroService;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<ResultadoPaginado> ListarLivros(string? titulo = null, string? autor = null, bool? Disponivel = null, int page = 1, int pageSize = 8)
        {
            return Ok(_livroService.ListarLivros(titulo, autor, Disponivel, page, pageSize));
        }

        [HttpGet("{id}")]
        [AllowAnonymous]

        public ActionResult<Livro> BuscarPorId(int id)
        {
            var livro = _livroService.BuscarPorId(id);

            if (livro == null)
            {
                return NotFound();
            }

            return Ok(livro);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<Livro> AdicionarLivro([FromBody] LivroCriacaoDTO livroDTO)
        {
            Livro livro = new Livro
            {
                Ano = livroDTO.Ano,
                Autores = livroDTO.Autores
                .Select(nome => new Autor {Nome = nome})
                .ToList(),
                Titulo = livroDTO.Titulo
            };

            var NovoLivro = _livroService.AdicionarLivro(livro);
            if (NovoLivro == false)
            {
                return BadRequest("Não foi possível adicionar o livro.");
            }
            return CreatedAtAction(
                "BuscarPorId",
                new { id = livro.ID },
                livro
            );
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult<Livro> AtualizarLivro(int id, LivroCriacaoDTO livroDTO)
        {
            if (_livroService.BuscarPorId(id) == null)
            {
                return NotFound("livro não encontrado");
            }

            var livroAtualizado = _livroService.Update(id, livroDTO.Titulo, livroDTO.Autores.Select(nome => new Autor {Nome = nome }).ToList(), livroDTO.Ano);

            if (livroAtualizado == false)
            {
                return BadRequest("Dados inválidos para atualizar o livro.");

            }
            return Ok(_livroService.BuscarPorId(id));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult<Livro> ExcluirLivro(int id)
        {
            var livro = _livroService.BuscarPorId(id);

            if (livro == null)
            {
                return NotFound("Livro não encontrado");
            }

            var LivroExcluido = _livroService.RemoverLivro(id);

            if (LivroExcluido == false)
            {
                return BadRequest("Não é possível excluir um livro que possui empréstimo.");
            }

            return NoContent();
        }

        [HttpGet("PesquisarGoogle")]
        [AllowAnonymous]
        public async Task<IActionResult> PesquisrGoogle(string termo)
        {
            var Resultado = await _googleBooksService.PesquisarLivros(termo);
            return Ok(Resultado);
        }
    }
    
}