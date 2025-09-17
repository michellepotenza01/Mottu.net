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
    [Tags("P�tios")]
    [Produces("application/json")]
    public class PatioController : ControllerBase
    {
        private readonly PatioService _patioService;

        public PatioController(PatioService patioService)
        {
            _patioService = patioService;
        }

        /// <summary>
        /// Retorna todos os p�tios com pagina��o.
        /// </summary>
        /// <param name="page">N�mero da p�gina (padr�o: 1)</param>
        /// <param name="pageSize">Itens por p�gina (padr�o: 10, m�ximo: 50)</param>
        [HttpGet]
        [SwaggerOperation(Summary = "Obter todos os p�tios com pagina��o")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResponse<Patio>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        public async Task<ActionResult> GetPatios(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            if (page < 1)
                return BadRequest(new ErrorResponse { Message = "O n�mero da p�gina deve ser maior que 0." });

            if (pageSize < 1 || pageSize > 50)
                return BadRequest(new ErrorResponse { Message = "O tamanho da p�gina deve estar entre 1 e 50." });

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

            // Links de pagina��o
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
        /// Retorna um p�tio espec�fico pelo nome.
        /// </summary>
        /// <param name="nomePatio">Nome do p�tio</param>
        [HttpGet("{nomePatio}")]
        [SwaggerOperation(Summary = "Obter p�tio por nome")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PatioResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        public async Task<ActionResult> GetPatioByName([FromRoute] string nomePatio)
        {
            if (string.IsNullOrEmpty(nomePatio))
                return BadRequest(new ErrorResponse { Message = "Nome do p�tio � obrigat�rio." });

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
        /// Cria um novo p�tio no sistema.
        /// </summary>
        [HttpPost]
        [SwaggerOperation(Summary = "Criar novo p�tio")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PatioResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        public async Task<ActionResult> CreatePatio([FromBody] PatioDto patioDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ErrorResponse
                {
                    Message = "Dados inv�lidos",
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
        /// Atualiza um p�tio existente.
        /// </summary>
        /// <param name="nomePatio">Nome do p�tio a ser atualizado</param>
        /// <param name="patioDto">Dados atualizados do p�tio</param>
        [HttpPut("{nomePatio}")]
        [SwaggerOperation(Summary = "Atualizar p�tio existente")]
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
                    Message = "Dados inv�lidos",
                    Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                });

            if (nomePatio != patioDto.NomePatio)
                return BadRequest(new ErrorResponse { Message = "O nome do p�tio na URL n�o corresponde ao nome do corpo da requisi��o." });

            var response = await _patioService.UpdatePatioAsync(nomePatio, patioDto);

            if (!response.Success)
            {
                if (response.Message.Contains("n�o encontrado"))
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
        /// Exclui um p�tio do sistema.
        /// </summary>
        /// <param name="nomePatio">Nome do p�tio a ser exclu�do</param>
        [HttpDelete("{nomePatio}")]
        [SwaggerOperation(Summary = "Excluir p�tio")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> DeletePatio([FromRoute] string nomePatio)
        {
            if (string.IsNullOrEmpty(nomePatio))
                return BadRequest(new ErrorResponse { Message = "Nome do p�tio � obrigat�rio." });

            var response = await _patioService.DeletePatioAsync(nomePatio);

            if (!response.Success)
            {
                if (response.Message.Contains("n�o encontrado"))
                    return NotFound(new ErrorResponse { Message = response.Message });

                return BadRequest(new ErrorResponse { Message = response.Message });
            }

            return NoContent();
        }

        /// <summary>
        /// Obt�m a quantidade de vagas dispon�veis em um p�tio.
        /// </summary>
        /// <param name="nomePatio">Nome do p�tio</param>
        [HttpGet("{nomePatio}/vagas")]
        [SwaggerOperation(Summary = "Obter vagas dispon�veis no p�tio")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VagasResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        public async Task<ActionResult> GetVagasDisponiveis([FromRoute] string nomePatio)
        {
            if (string.IsNullOrEmpty(nomePatio))
                return BadRequest(new ErrorResponse { Message = "Nome do p�tio � obrigat�rio." });

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