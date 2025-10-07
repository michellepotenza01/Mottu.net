using Microsoft.AspNetCore.Mvc;
using MottuApi.Models;
using MottuApi.DTOs;
using MottuApi.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Filters;
using MottuApi.Examples;
using MottuApi3.Enums;

namespace MottuApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Tags("Motos")]
    [Produces("application/json")]
    public class MotoController : ControllerBase
    {
        private readonly MotoService _motoService;

        public MotoController(MotoService motoService)
        {
            _motoService = motoService;
        }

        /// <summary>
        /// Retorna todas as motos com paginacao e filtros opcionais.
        /// </summary>
        /// <param name="page">Numero da pagina (padrao: 1)</param>
        /// <param name="pageSize">Itens por pagina (padrao: 10, maximo: 50)</param>
        /// <param name="status">Filtrar por status da moto</param>
        /// <param name="setor">Filtrar por setor de conservacao</param>
        [HttpGet]
        [SwaggerOperation(Summary = "Obter todas as motos com paginacao")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResponse<Moto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        public async Task<ActionResult> GetMotos(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] StatusMoto? status = null,
            [FromQuery] SetorMoto? setor = null)
        {
            if (page < 1)
                return BadRequest(new ErrorResponse { Message = "O numero da pagina deve ser maior que 0." });

            if (pageSize < 1 || pageSize > 50)
                return BadRequest(new ErrorResponse { Message = "O tamanho da pagina deve estar entre 1 e 50." });

            var motosResponse = await _motoService.GetMotosPaginatedAsync(page, pageSize, status, setor);
            var totalCountResponse = await _motoService.GetTotalMotosCountAsync(status, setor);

            if (!motosResponse.Success || !totalCountResponse.Success)
                return BadRequest(new ErrorResponse { Message = motosResponse.Message });

            if (motosResponse.Data == null || !motosResponse.Data.Any())
                return NoContent();

            var totalCount = totalCountResponse.Data;
            var totalPages = (int)System.Math.Ceiling(totalCount / (double)pageSize);

            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
            var links = new List<Link>
            {
                new Link { Rel = "self", Href = $"{baseUrl}/api/moto?page={page}&pageSize={pageSize}", Method = "GET" },
                new Link { Rel = "create", Href = $"{baseUrl}/api/moto", Method = "POST" }
            };

            if (page > 1)
                links.Add(new Link { Rel = "prev", Href = $"{baseUrl}/api/moto?page={page - 1}&pageSize={pageSize}", Method = "GET" });

            if (page < totalPages)
                links.Add(new Link { Rel = "next", Href = $"{baseUrl}/api/moto?page={page + 1}&pageSize={pageSize}", Method = "GET" });

            var response = new PagedResponse<Moto>
            {
                Data = motosResponse.Data,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                Links = links
            };

            return Ok(response);
        }

        /// <summary>
        /// Retorna uma moto especifica pela placa.
        /// </summary>
        /// <param name="placa">Placa da moto no formato XXX-0000</param>
        [HttpGet("{placa}")]
        [SwaggerOperation(Summary = "Obter moto por placa")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MotoResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        public async Task<ActionResult> GetMotoByPlaca([FromRoute] string placa)
        {
            if (string.IsNullOrEmpty(placa) || !System.Text.RegularExpressions.Regex.IsMatch(placa, @"^[A-Z]{3}-\d{4}$"))
                return BadRequest(new ErrorResponse { Message = "Formato de placa inv�lido. Use: XXX-0000" });

            var response = await _motoService.GetMotoAsync(placa);

            if (!response.Success || response.Data == null)
                return NotFound(new ErrorResponse { Message = response.Message });

            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

            var motoResponse = new MotoResponse
            {
                Placa = response.Data.Placa,
                Modelo = response.Data.Modelo,
                Status = response.Data.Status,
                Setor = response.Data.Setor,
                NomePatio = response.Data.NomePatio,
                UsuarioFuncionario = response.Data.UsuarioFuncionario,
                Links = new List<Link>
                {
                    new Link { Rel = "self", Href = $"{baseUrl}/api/moto/{placa}", Method = "GET" },
                    new Link { Rel = "update", Href = $"{baseUrl}/api/moto/{placa}", Method = "PUT" },
                    new Link { Rel = "delete", Href = $"{baseUrl}/api/moto/{placa}", Method = "DELETE" },
                    new Link { Rel = "all", Href = $"{baseUrl}/api/moto", Method = "GET" }
                }
            };

            return Ok(motoResponse);
        }

        /// <summary>
        /// Cria uma nova moto no sistema.
        /// </summary>
        [HttpPost]
        [SwaggerRequestExample(typeof(MotoDto), typeof(MotoExample))]
        [SwaggerOperation(Summary = "Criar nova moto")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(MotoResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        public async Task<ActionResult> CreateMoto([FromBody] MotoDto motoDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ErrorResponse
                {
                    Message = "Dados invalidos",
                    Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                });

            var response = await _motoService.CreateMotoAsync(motoDto);

            if (!response.Success)
            {
                if (response.Message.Contains("nao encontrado"))
                    return NotFound(new ErrorResponse { Message = response.Message });

                return BadRequest(new ErrorResponse { Message = response.Message });
            }

            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

            var motoResponse = new MotoResponse
            {
                Placa = response.Data.Placa,
                Modelo = response.Data.Modelo,
                Status = response.Data.Status,
                Setor = response.Data.Setor,
                NomePatio = response.Data.NomePatio,
                UsuarioFuncionario = response.Data.UsuarioFuncionario,
                Links = new List<Link>
                {
                    new Link { Rel = "self", Href = $"{baseUrl}/api/moto/{response.Data.Placa}", Method = "GET" },
                    new Link { Rel = "update", Href = $"{baseUrl}/api/moto/{response.Data.Placa}", Method = "PUT" },
                    new Link { Rel = "delete", Href = $"{baseUrl}/api/moto/{response.Data.Placa}", Method = "DELETE" },
                    new Link { Rel = "all", Href = $"{baseUrl}/api/moto", Method = "GET" }
                }
            };

            return CreatedAtAction(
                nameof(GetMotoByPlaca),
                new { placa = response.Data.Placa },
                motoResponse
            );
        }

        /// <summary>
        /// Atualiza uma moto existente.
        /// </summary>
        /// <param name="placa">Placa da moto a ser atualizada</param>
        /// <param name="motoDto">Dados atualizados da moto</param>
        [HttpPut("{placa}")]
        [SwaggerOperation(Summary = "Atualizar moto existente")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MotoResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        public async Task<ActionResult> UpdateMoto(
            [FromRoute] string placa,
            [FromBody] MotoDto motoDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ErrorResponse
                {
                    Message = "Dados invalidos",
                    Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                });

            if (placa != motoDto.Placa)
                return BadRequest(new ErrorResponse { Message = "A placa da URL não corresponde à placa do corpo da requisicao." });

            var response = await _motoService.UpdateMotoAsync(placa, motoDto);

            if (!response.Success)
            {
                if (response.Message.Contains("nao encontrada"))
                    return NotFound(new ErrorResponse { Message = response.Message });

                return BadRequest(new ErrorResponse { Message = response.Message });
            }

            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

            var motoResponse = new MotoResponse
            {
                Placa = response.Data.Placa,
                Modelo = response.Data.Modelo,
                Status = response.Data.Status,
                Setor = response.Data.Setor,
                NomePatio = response.Data.NomePatio,
                UsuarioFuncionario = response.Data.UsuarioFuncionario,
                Links = new List<Link>
                {
                    new Link { Rel = "self", Href = $"{baseUrl}/api/moto/{response.Data.Placa}", Method = "GET" },
                    new Link { Rel = "update", Href = $"{baseUrl}/api/moto/{response.Data.Placa}", Method = "PUT" },
                    new Link { Rel = "delete", Href = $"{baseUrl}/api/moto/{response.Data.Placa}", Method = "DELETE" },
                    new Link { Rel = "all", Href = $"{baseUrl}/api/moto", Method = "GET" }
                }
            };

            return Ok(motoResponse);
        }

        /// <summary>
        /// Exclui uma moto do sistema.
        /// </summary>
        /// <param name="placa">Placa da moto a ser exclu�da</param>
        [HttpDelete("{placa}")]
        [SwaggerOperation(Summary = "Excluir moto")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> DeleteMoto([FromRoute] string placa)
        {
            if (string.IsNullOrEmpty(placa) || !System.Text.RegularExpressions.Regex.IsMatch(placa, @"^[A-Z]{3}-\d{4}$"))
                return BadRequest(new ErrorResponse { Message = "Formato de placa invalido. Use: XXX-0000" });

            var response = await _motoService.DeleteMotoAsync(placa);

            if (!response.Success)
            {
                if (response.Message.Contains("nao encontrada"))
                    return NotFound(new ErrorResponse { Message = response.Message });

                return BadRequest(new ErrorResponse { Message = response.Message });
            }

            return NoContent();
        }
    }

    public class MotoResponse
    {
        public string Placa { get; set; } = string.Empty;
        public ModeloMoto Modelo { get; set; }
        public StatusMoto Status { get; set; }
        public SetorMoto Setor { get; set; }
        public string NomePatio { get; set; } = string.Empty;
        public string UsuarioFuncionario { get; set; } = string.Empty;
        public List<Link> Links { get; set; } = new List<Link>();
    }
}