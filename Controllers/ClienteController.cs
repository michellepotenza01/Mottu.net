using Microsoft.AspNetCore.Mvc;
using MottuApi.Models;
using MottuApi.DTOs;
using MottuApi.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MottuApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Tags("Clientes")]
    [Produces("application/json")]
    public class ClienteController : ControllerBase
    {
        private readonly ClienteService _clienteService;

        public ClienteController(ClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        /// <summary>
        /// Retorna todos os clientes com paginação.
        /// </summary>
        /// <param name="page">Número da página (padrão: 1)</param>
        /// <param name="pageSize">Itens por página (padrão: 10, máximo: 50)</param>
        [HttpGet]
        [SwaggerOperation(Summary = "Obter todos os clientes com paginação")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResponse<Cliente>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        public async Task<ActionResult> GetClientes(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            if (page < 1)
                return BadRequest(new ErrorResponse { Message = "O número da página deve ser maior que 0." });

            if (pageSize < 1 || pageSize > 50)
                return BadRequest(new ErrorResponse { Message = "O tamanho da página deve estar entre 1 and 50." });

            var clientesResponse = await _clienteService.GetClientesPaginatedAsync(page, pageSize);
            var totalCountResponse = await _clienteService.GetTotalClientesCountAsync();

            if (!clientesResponse.Success || !totalCountResponse.Success)
                return BadRequest(new ErrorResponse { Message = clientesResponse.Message });

            if (clientesResponse.Data == null || !clientesResponse.Data.Any())
                return NoContent();

            var totalCount = totalCountResponse.Data;
            var totalPages = (int)System.Math.Ceiling(totalCount / (double)pageSize);

            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
            var links = new List<Link>
            {
                new Link { Rel = "self", Href = $"{baseUrl}/api/cliente?page={page}&pageSize={pageSize}", Method = "GET" },
                new Link { Rel = "create", Href = $"{baseUrl}/api/cliente", Method = "POST" }
            };

            if (page > 1)
                links.Add(new Link { Rel = "prev", Href = $"{baseUrl}/api/cliente?page={page - 1}&pageSize={pageSize}", Method = "GET" });

            if (page < totalPages)
                links.Add(new Link { Rel = "next", Href = $"{baseUrl}/api/cliente?page={page + 1}&pageSize={pageSize}", Method = "GET" });

            var response = new PagedResponse<Cliente>
            {
                Data = clientesResponse.Data,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                Links = links
            };

            return Ok(response);
        }

        /// <summary>
        /// Retorna um cliente específico pelo usuário.
        /// </summary>
        /// <param name="usuarioCliente">Usuário do cliente</param>
        [HttpGet("{usuarioCliente}")]
        [SwaggerOperation(Summary = "Obter cliente por usuário")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ClienteResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        public async Task<ActionResult> GetClienteByUsuario([FromRoute] string usuarioCliente)
        {
            if (string.IsNullOrEmpty(usuarioCliente))
                return BadRequest(new ErrorResponse { Message = "Usuário do cliente é obrigatório." });

            var response = await _clienteService.GetClienteByIdAsync(usuarioCliente);

            if (!response.Success || response.Data == null)
                return NotFound(new ErrorResponse { Message = response.Message });

            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

            var clienteResponse = new ClienteResponse
            {
                UsuarioCliente = response.Data.UsuarioCliente,
                Nome = response.Data.Nome,
                MotoPlaca = response.Data.MotoPlaca,
                Links = new List<Link>
                {
                    new Link { Rel = "self", Href = $"{baseUrl}/api/cliente/{usuarioCliente}", Method = "GET" },
                    new Link { Rel = "update", Href = $"{baseUrl}/api/cliente/{usuarioCliente}", Method = "PUT" },
                    new Link { Rel = "delete", Href = $"{baseUrl}/api/cliente/{usuarioCliente}", Method = "DELETE" },
                    new Link { Rel = "all", Href = $"{baseUrl}/api/cliente", Method = "GET" }
                }
            };

            return Ok(clienteResponse);
        }

        /// <summary>
        /// Cria um novo cliente no sistema.
        /// </summary>
        [HttpPost]
        [SwaggerOperation(Summary = "Criar novo cliente")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ClienteResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        public async Task<ActionResult> CreateCliente([FromBody] ClienteDto clienteDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ErrorResponse
                {
                    Message = "Dados inválidos",
                    Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                });

            var response = await _clienteService.CreateClienteAsync(clienteDto);

            if (!response.Success)
            {
                if (response.Message.Contains("não encontrada"))
                    return NotFound(new ErrorResponse { Message = response.Message });

                return BadRequest(new ErrorResponse { Message = response.Message });
            }

            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

            var clienteResponse = new ClienteResponse
            {
                UsuarioCliente = response.Data.UsuarioCliente,
                Nome = response.Data.Nome,
                MotoPlaca = response.Data.MotoPlaca,
                Links = new List<Link>
                {
                    new Link { Rel = "self", Href = $"{baseUrl}/api/cliente/{response.Data.UsuarioCliente}", Method = "GET" },
                    new Link { Rel = "update", Href = $"{baseUrl}/api/cliente/{response.Data.UsuarioCliente}", Method = "PUT" },
                    new Link { Rel = "delete", Href = $"{baseUrl}/api/cliente/{response.Data.UsuarioCliente}", Method = "DELETE" },
                    new Link { Rel = "all", Href = $"{baseUrl}/api/cliente", Method = "GET" }
                }
            };

            return CreatedAtAction(
                nameof(GetClienteByUsuario),
                new { usuarioCliente = response.Data.UsuarioCliente },
                clienteResponse
            );
        }

        /// <summary>
        /// Atualiza um cliente existente.
        /// </summary>
        /// <param name="usuarioCliente">Usuário do cliente a ser atualizado</param>
        /// <param name="clienteDto">Dados atualizados do cliente</param>
        [HttpPut("{usuarioCliente}")]
        [SwaggerOperation(Summary = "Atualizar cliente existente")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ClienteResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        public async Task<ActionResult> UpdateCliente(
            [FromRoute] string usuarioCliente,
            [FromBody] ClienteDto clienteDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ErrorResponse
                {
                    Message = "Dados inválidos",
                    Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                });

            if (usuarioCliente != clienteDto.UsuarioCliente)
                return BadRequest(new ErrorResponse { Message = "O usuário do cliente na URL não corresponde ao usuário do corpo da requisição." });

            var response = await _clienteService.UpdateClienteAsync(usuarioCliente, clienteDto);

            if (!response.Success)
            {
                if (response.Message.Contains("não encontrado"))
                    return NotFound(new ErrorResponse { Message = response.Message });

                return BadRequest(new ErrorResponse { Message = response.Message });
            }

            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

            var clienteResponse = new ClienteResponse
            {
                UsuarioCliente = response.Data.UsuarioCliente,
                Nome = response.Data.Nome,
                MotoPlaca = response.Data.MotoPlaca,
                Links = new List<Link>
                {
                    new Link { Rel = "self", Href = $"{baseUrl}/api/cliente/{response.Data.UsuarioCliente}", Method = "GET" },
                    new Link { Rel = "update", Href = $"{baseUrl}/api/cliente/{response.Data.UsuarioCliente}", Method = "PUT" },
                    new Link { Rel = "delete", Href = $"{baseUrl}/api/cliente/{response.Data.UsuarioCliente}", Method = "DELETE" },
                    new Link { Rel = "all", Href = $"{baseUrl}/api/cliente", Method = "GET" }
                }
            };

            return Ok(clienteResponse);
        }

        /// <summary>
        /// Exclui um cliente do sistema.
        /// </summary>
        /// <param name="usuarioCliente">Usuário do cliente a ser excluído</param>
        [HttpDelete("{usuarioCliente}")]
        [SwaggerOperation(Summary = "Excluir cliente")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> DeleteCliente([FromRoute] string usuarioCliente)
        {
            if (string.IsNullOrEmpty(usuarioCliente))
                return BadRequest(new ErrorResponse { Message = "Usuário do cliente é obrigatório." });

            var response = await _clienteService.DeleteClienteAsync(usuarioCliente);

            if (!response.Success)
            {
                if (response.Message.Contains("não encontrado"))
                    return NotFound(new ErrorResponse { Message = response.Message });

                return BadRequest(new ErrorResponse { Message = response.Message });
            }

            return NoContent();
        }
    }

    public class ClienteResponse
    {
        public string UsuarioCliente { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string? MotoPlaca { get; set; }
        public List<Link> Links { get; set; } = new List<Link>();
    }
}

public class PagedResponse<T>
{
    public List<T> Data { get; set; } = new List<T>();
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public List<Link> Links { get; set; } = new List<Link>();
}

public class Link
{
    public string Rel { get; set; } = string.Empty;
    public string Href { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;
}

public class ErrorResponse
{
    public string Message { get; set; } = string.Empty;
    public List<string> Errors { get; set; } = new List<string>();
}