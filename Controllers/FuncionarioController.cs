using Microsoft.AspNetCore.Mvc;
using MottuApi.Models;
using MottuApi.DTOs;
using MottuApi.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MottuApi.Enums;


namespace MottuApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Tags("Funcion�rios")]
    [Produces("application/json")]
    public class FuncionarioController : ControllerBase
    {
        private readonly FuncionarioService _funcionarioService;

        public FuncionarioController(FuncionarioService funcionarioService)
        {
            _funcionarioService = funcionarioService;
        }

        /// <summary>
        /// Retorna todos os funcion�rios com pagina��o.
        /// </summary>
        /// <param name="page">N�mero da p�gina (padr�o: 1)</param>
        /// <param name="pageSize">Itens por p�gina (padr�o: 10, m�ximo: 50)</param>
        [HttpGet]
        [SwaggerOperation(Summary = "Obter todos os funcion�rios com pagina��o")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResponse<Funcionario>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        public async Task<ActionResult> GetFuncionarios(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            if (page < 1)
                return BadRequest(new ErrorResponse { Message = "O n�mero da p�gina deve ser maior que 0." });

            if (pageSize < 1 || pageSize > 50)
                return BadRequest(new ErrorResponse { Message = "O tamanho da p�gina deve estar entre 1 e 50." });

            var funcionariosResponse = await _funcionarioService.GetFuncionariosPaginatedAsync(page, pageSize);
            var totalCountResponse = await _funcionarioService.GetTotalFuncionariosCountAsync();

            if (!funcionariosResponse.Success || !totalCountResponse.Success)
                return BadRequest(new ErrorResponse { Message = funcionariosResponse.Message });

            if (funcionariosResponse.Data == null || !funcionariosResponse.Data.Any())
                return NoContent();

            var totalCount = totalCountResponse.Data;
            var totalPages = (int)System.Math.Ceiling(totalCount / (double)pageSize);

            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
            var links = new List<Link>
            {
                new Link { Rel = "self", Href = $"{baseUrl}/api/funcionario?page={page}&pageSize={pageSize}", Method = "GET" },
                new Link { Rel = "create", Href = $"{baseUrl}/api/funcionario", Method = "POST" }
            };

            if (page > 1)
                links.Add(new Link { Rel = "prev", Href = $"{baseUrl}/api/funcionario?page={page - 1}&pageSize={pageSize}", Method = "GET" });

            if (page < totalPages)
                links.Add(new Link { Rel = "next", Href = $"{baseUrl}/api/funcionario?page={page + 1}&pageSize={pageSize}", Method = "GET" });

            var response = new PagedResponse<Funcionario>
            {
                Data = funcionariosResponse.Data,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                Links = links
            };

            return Ok(response);
        }

        /// <summary>
        /// Retorna um funcion�rio espec�fico pelo usu�rio.
        /// </summary>
        /// <param name="usuarioFuncionario">Usu�rio do funcion�rio</param>
        [HttpGet("{usuarioFuncionario}")]
        [SwaggerOperation(Summary = "Obter funcion�rio por usu�rio")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FuncionarioResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        public async Task<ActionResult> GetFuncionarioByUsuario([FromRoute] string usuarioFuncionario)
        {
            if (string.IsNullOrEmpty(usuarioFuncionario))
                return BadRequest(new ErrorResponse { Message = "Usu�rio do funcion�rio � obrigat�rio." });

            var response = await _funcionarioService.GetFuncionarioByIdAsync(usuarioFuncionario);

            if (!response.Success || response.Data == null)
                return NotFound(new ErrorResponse { Message = response.Message });

            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

            var funcionarioResponse = new FuncionarioResponse
            {
                UsuarioFuncionario = response.Data.UsuarioFuncionario,
                Nome = response.Data.Nome,
                NomePatio = response.Data.NomePatio,
                Links = new List<Link>
                {
                    new Link { Rel = "self", Href = $"{baseUrl}/api/funcionario/{usuarioFuncionario}", Method = "GET" },
                    new Link { Rel = "update", Href = $"{baseUrl}/api/funcionario/{usuarioFuncionario}", Method = "PUT" },
                    new Link { Rel = "delete", Href = $"{baseUrl}/api/funcionario/{usuarioFuncionario}", Method = "DELETE" },
                    new Link { Rel = "all", Href = $"{baseUrl}/api/funcionario", Method = "GET" }
                }
            };

            return Ok(funcionarioResponse);
        }

        /// <summary>
        /// Cria um novo funcion�rio no sistema.
        /// </summary>
        [HttpPost]
        [SwaggerOperation(Summary = "Criar novo funcion�rio")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(FuncionarioResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        public async Task<ActionResult> CreateFuncionario([FromBody] FuncionarioDto funcionarioDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ErrorResponse
                {
                    Message = "Dados inv�lidos",
                    Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                });

            var response = await _funcionarioService.CreateFuncionarioAsync(funcionarioDto);

            if (!response.Success)
            {
                if (response.Message.Contains("n�o encontrado"))
                    return NotFound(new ErrorResponse { Message = response.Message });

                return BadRequest(new ErrorResponse { Message = response.Message });
            }

            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

            var funcionarioResponse = new FuncionarioResponse
            {
                UsuarioFuncionario = response.Data.UsuarioFuncionario,
                Nome = response.Data.Nome,
                NomePatio = response.Data.NomePatio,
                Links = new List<Link>
                {
                    new Link { Rel = "self", Href = $"{baseUrl}/api/funcionario/{response.Data.UsuarioFuncionario}", Method = "GET" },
                    new Link { Rel = "update", Href = $"{baseUrl}/api/funcionario/{response.Data.UsuarioFuncionario}", Method = "PUT" },
                    new Link { Rel = "delete", Href = $"{baseUrl}/api/funcionario/{response.Data.UsuarioFuncionario}", Method = "DELETE" },
                    new Link { Rel = "all", Href = $"{baseUrl}/api/funcionario", Method = "GET" }
                }
            };

            return CreatedAtAction(
                nameof(GetFuncionarioByUsuario),
                new { usuarioFuncionario = response.Data.UsuarioFuncionario },
                funcionarioResponse
            );
        }

        /// <summary>
        /// Atualiza um funcion�rio existente.
        /// </summary>
        /// <param name="usuarioFuncionario">Usu�rio do funcion�rio a ser atualizado</param>
        /// <param name="funcionarioDto">Dados atualizados do funcion�rio</param>
        [HttpPut("{usuarioFuncionario}")]
        [SwaggerOperation(Summary = "Atualizar funcion�rio existente")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FuncionarioResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        public async Task<ActionResult> UpdateFuncionario(
            [FromRoute] string usuarioFuncionario,
            [FromBody] FuncionarioDto funcionarioDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ErrorResponse
                {
                    Message = "Dados inv�lidos",
                    Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                });

            if (usuarioFuncionario != funcionarioDto.UsuarioFuncionario)
                return BadRequest(new ErrorResponse { Message = "O usu�rio do funcion�rio na URL n�o corresponde ao usu�rio do corpo da requisi��o." });

            var response = await _funcionarioService.UpdateFuncionarioAsync(usuarioFuncionario, funcionarioDto);

            if (!response.Success)
            {
                if (response.Message.Contains("n�o encontrado"))
                    return NotFound(new ErrorResponse { Message = response.Message });

                return BadRequest(new ErrorResponse { Message = response.Message });
            }

            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

            var funcionarioResponse = new FuncionarioResponse
            {
                UsuarioFuncionario = response.Data.UsuarioFuncionario,
                Nome = response.Data.Nome,
                NomePatio = response.Data.NomePatio,
                Links = new List<Link>
                {
                    new Link { Rel = "self", Href = $"{baseUrl}/api/funcionario/{response.Data.UsuarioFuncionario}", Method = "GET" },
                    new Link { Rel = "update", Href = $"{baseUrl}/api/funcionario/{response.Data.UsuarioFuncionario}", Method = "PUT" },
                    new Link { Rel = "delete", Href = $"{baseUrl}/api/funcionario/{response.Data.UsuarioFuncionario}", Method = "DELETE" },
                    new Link { Rel = "all", Href = $"{baseUrl}/api/funcionario", Method = "GET" }
                }
            };

            return Ok(funcionarioResponse);
        }

        /// <summary>
        /// Exclui um funcion�rio do sistema.
        /// </summary>
        /// <param name="usuarioFuncionario">Usu�rio do funcion�rio a ser exclu�do</param>
        [HttpDelete("{usuarioFuncionario}")]
        [SwaggerOperation(Summary = "Excluir funcion�rio")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> DeleteFuncionario([FromRoute] string usuarioFuncionario)
        {
            if (string.IsNullOrEmpty(usuarioFuncionario))
                return BadRequest(new ErrorResponse { Message = "Usu�rio do funcion�rio � obrigat�rio." });

            var response = await _funcionarioService.DeleteFuncionarioAsync(usuarioFuncionario);

            if (!response.Success)
            {
                if (response.Message.Contains("n�o encontrado"))
                    return NotFound(new ErrorResponse { Message = response.Message });

                return BadRequest(new ErrorResponse { Message = response.Message });
            }

            return NoContent();
        }
    }

    public class FuncionarioResponse
    {
        public string UsuarioFuncionario { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string NomePatio { get; set; } = string.Empty;
        public List<Link> Links { get; set; } = new List<Link>();
    }
}
