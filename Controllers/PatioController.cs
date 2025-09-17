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
    [Tags("Pátios")]
    [Produces("application/json")]
    public class PatioController : ControllerBase
    {
        private readonly PatioService _patioService;

        public PatioController(PatioService patioService)
        {
            _patioService = patioService;
        }

        /// <summary>
        /// Retorna todos os pátios com paginação.
        /// </summary>
        /// <param name="page">Número da página (padrão: 1)</param>
        /// <param name="pageSize">Itens por página (padrão: 10, máximo: 50)</param>
        [HttpGet]
        [SwaggerOperation(Summary = "Obter todos os pátios com paginação")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResponse<Patio>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        public async Task<ActionResult> GetPatios(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            if (page < 1)
                return BadRequest(new ErrorResponse { Message = "O número da página deve ser maior que 0." });

            if (pageSize < 1 || pageSize > 50)
                return BadRequest(new ErrorResponse { Message = "O tamanho da página deve estar entre 1 e 50." });

            var patiosResponse = await _patioService.GetPatiosPaginatedAsync(page, pageSize);
            var totalCountResponse = await _patioService.GetTotalPatiosCountAsync();

            if (!patiosResponse.Success || !totalCountResponse.Success)
                return BadRequest(new ErrorResponse { Message = patiosResponse.Message });

            if (patiosResponse.Data == null || !patiosResponse.Data.Any())
                return NoContent();

            var totalCount = totalCountResponse.Data;
            var totalPages = (int)System.Math.Ceiling(totalCount / (double)pageSize);

            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
            var links = new List<Link>
            {
                new Link { Rel = "self", Href = $"{baseUrl}/api/patio?page={page}&pageSize={pageSize}", Method = "GET" },
                new Link { Rel = "create", Href = $"{baseUrl}/api/patio", Method = "POST" }
            };

            // Links de paginação
            if (page > 1)
                links.Add(new Link { Rel = "prev", Href = $"{baseUrl}/api/patio?page={page - 1}&pageSize={pageSize}", Method = "GET" });

            if (page < totalPages)
                links.Add(new Link { Rel = "next", Href = $"{baseUrl}/api/patio?page={page + 1}&pageSize={pageSize}", Method = "GET" });

            var response = new PagedResponse<Patio>
            {
                Data = patiosResponse.Data,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                Links = links
            };

            return Ok(response);
        }

        /// <summary>
        /// Retorna um pátio específico pelo nome.
        /// </summary>
        /// <param name="nomePatio">Nome do pátio</param>
        [HttpGet("{nomePatio}")]
        [SwaggerOperation(Summary = "Obter pátio por nome")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PatioResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        public async Task<ActionResult> GetPatioByName([FromRoute] string nomePatio)
        {
            if (string.IsNullOrEmpty(nomePatio))
                return BadRequest(new ErrorResponse { Message = "Nome do pátio é obrigatório." });

            var response = await _patioService.GetPatioAsync(nomePatio);

            if (!response.Success || response.Data == null)
                return NotFound(new ErrorResponse { Message = response.Message });

            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

            var patioResponse = new PatioResponse
            {
                NomePatio = response.Data.NomePatio,
                Localizacao = response.Data.Localizacao,
                VagasTotais = response.Data.VagasTotais,
                VagasOcupadas = response.Data.VagasOcupadas,
                VagasDisponiveis = response.Data.VagasDisponiveis,
                Links = new List<Link>
                {
                    new Link { Rel = "self", Href = $"{baseUrl}/api/patio/{nomePatio}", Method = "GET" },
                    new Link { Rel = "update", Href = $"{baseUrl}/api/patio/{nomePatio}", Method = "PUT" },
                    new Link { Rel = "delete", Href = $"{baseUrl}/api/patio/{nomePatio}", Method = "DELETE" },
                    new Link { Rel = "all", Href = $"{baseUrl}/api/patio", Method = "GET" },
                    new Link { Rel = "vagas", Href = $"{baseUrl}/api/patio/{nomePatio}/vagas", Method = "GET" }
                }
            };

            return Ok(patioResponse);
        }

        /// <summary>
        /// Cria um novo pátio no sistema.
        /// </summary>
        [HttpPost]
        [SwaggerOperation(Summary = "Criar novo pátio")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PatioResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        public async Task<ActionResult> CreatePatio([FromBody] PatioDto patioDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ErrorResponse
                {
                    Message = "Dados inválidos",
                    Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                });

            var response = await _patioService.CreatePatioAsync(patioDto);

            if (!response.Success)
                return BadRequest(new ErrorResponse { Message = response.Message });

            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

            var patioResponse = new PatioResponse
            {
                NomePatio = response.Data.NomePatio,
                Localizacao = response.Data.Localizacao,
                VagasTotais = response.Data.VagasTotais,
                VagasOcupadas = response.Data.VagasOcupadas,
                VagasDisponiveis = response.Data.VagasDisponiveis,
                Links = new List<Link>
                {
                    new Link { Rel = "self", Href = $"{baseUrl}/api/patio/{response.Data.NomePatio}", Method = "GET" },
                    new Link { Rel = "update", Href = $"{baseUrl}/api/patio/{response.Data.NomePatio}", Method = "PUT" },
                    new Link { Rel = "delete", Href = $"{baseUrl}/api/patio/{response.Data.NomePatio}", Method = "DELETE" },
                    new Link { Rel = "all", Href = $"{baseUrl}/api/patio", Method = "GET" },
                    new Link { Rel = "vagas", Href = $"{baseUrl}/api/patio/{response.Data.NomePatio}/vagas", Method = "GET" }
                }
            };

            return CreatedAtAction(
                nameof(GetPatioByName),
                new { nomePatio = response.Data.NomePatio },
                patioResponse
            );
        }

        /// <summary>
        /// Atualiza um pátio existente.
        /// </summary>
        /// <param name="nomePatio">Nome do pátio a ser atualizado</param>
        /// <param name="patioDto">Dados atualizados do pátio</param>
        [HttpPut("{nomePatio}")]
        [SwaggerOperation(Summary = "Atualizar pátio existente")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PatioResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        public async Task<ActionResult> UpdatePatio(
            [FromRoute] string nomePatio,
            [FromBody] PatioDto patioDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ErrorResponse
                {
                    Message = "Dados inválidos",
                    Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                });

            if (nomePatio != patioDto.NomePatio)
                return BadRequest(new ErrorResponse { Message = "O nome do pátio na URL não corresponde ao nome do corpo da requisição." });

            var response = await _patioService.UpdatePatioAsync(nomePatio, patioDto);

            if (!response.Success)
            {
                if (response.Message.Contains("não encontrado"))
                    return NotFound(new ErrorResponse { Message = response.Message });

                return BadRequest(new ErrorResponse { Message = response.Message });
            }

            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

            var patioResponse = new PatioResponse
            {
                NomePatio = response.Data.NomePatio,
                Localizacao = response.Data.Localizacao,
                VagasTotais = response.Data.VagasTotais,
                VagasOcupadas = response.Data.VagasOcupadas,
                VagasDisponiveis = response.Data.VagasDisponiveis,
                Links = new List<Link>
                {
                    new Link { Rel = "self", Href = $"{baseUrl}/api/patio/{response.Data.NomePatio}", Method = "GET" },
                    new Link { Rel = "update", Href = $"{baseUrl}/api/patio/{response.Data.NomePatio}", Method = "PUT" },
                    new Link { Rel = "delete", Href = $"{baseUrl}/api/patio/{response.Data.NomePatio}", Method = "DELETE" },
                    new Link { Rel = "all", Href = $"{baseUrl}/api/patio", Method = "GET" },
                    new Link { Rel = "vagas", Href = $"{baseUrl}/api/patio/{response.Data.NomePatio}/vagas", Method = "GET" }
                }
            };

            return Ok(patioResponse);
        }

        /// <summary>
        /// Exclui um pátio do sistema.
        /// </summary>
        /// <param name="nomePatio">Nome do pátio a ser excluído</param>
        [HttpDelete("{nomePatio}")]
        [SwaggerOperation(Summary = "Excluir pátio")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> DeletePatio([FromRoute] string nomePatio)
        {
            if (string.IsNullOrEmpty(nomePatio))
                return BadRequest(new ErrorResponse { Message = "Nome do pátio é obrigatório." });

            var response = await _patioService.DeletePatioAsync(nomePatio);

            if (!response.Success)
            {
                if (response.Message.Contains("não encontrado"))
                    return NotFound(new ErrorResponse { Message = response.Message });

                return BadRequest(new ErrorResponse { Message = response.Message });
            }

            return NoContent();
        }

        /// <summary>
        /// Obtém a quantidade de vagas disponíveis em um pátio.
        /// </summary>
        /// <param name="nomePatio">Nome do pátio</param>
        [HttpGet("{nomePatio}/vagas")]
        [SwaggerOperation(Summary = "Obter vagas disponíveis no pátio")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VagasResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        public async Task<ActionResult> GetVagasDisponiveis([FromRoute] string nomePatio)
        {
            if (string.IsNullOrEmpty(nomePatio))
                return BadRequest(new ErrorResponse { Message = "Nome do pátio é obrigatório." });

            var response = await _patioService.GetVagasDisponiveisAsync(nomePatio);

            if (!response.Success)
                return NotFound(new ErrorResponse { Message = response.Message });

            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

            var vagasResponse = new VagasResponse
            {
                NomePatio = nomePatio,
                VagasDisponiveis = response.Data,
                Links = new List<Link>
                {
                    new Link { Rel = "self", Href = $"{baseUrl}/api/patio/{nomePatio}/vagas", Method = "GET" },
                    new Link { Rel = "patio", Href = $"{baseUrl}/api/patio/{nomePatio}", Method = "GET" },
                    new Link { Rel = "all", Href = $"{baseUrl}/api/patio", Method = "GET" }
                }
            };

            return Ok(vagasResponse);
        }
    }

    public class PatioResponse
    {
        public string NomePatio { get; set; } = string.Empty;
        public string Localizacao { get; set; } = string.Empty;
        public int VagasTotais { get; set; }
        public int VagasOcupadas { get; set; }
        public int VagasDisponiveis { get; set; }
        public List<Link> Links { get; set; } = new List<Link>();
    }

    public class VagasResponse
    {
        public string NomePatio { get; set; } = string.Empty;
        public int VagasDisponiveis { get; set; }
        public List<Link> Links { get; set; } = new List<Link>();
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
}