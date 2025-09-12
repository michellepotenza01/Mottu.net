using Microsoft.AspNetCore.Mvc;
using MottuApi.Models;
using MottuApi.DTOs;
using MottuApi.Services;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MottuApi.Enums;

namespace MottuApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Tags("Motos")]
    public class MotoController : ControllerBase
    {
        private readonly IMotoService _motoService;

        public MotoController(IMotoService motoService)
        {
            _motoService = motoService;
        }

        /// <summary>
        /// Retorna todas as motos com paginação.
        /// </summary>
        /// <param name="page">Número da página (default: 1).</param>
        /// <param name="pageSize">Tamanho da página (default: 10).</param>
        /// <returns>Lista de motos paginada com dados de paginação.</returns>
        [HttpGet]
        [SwaggerOperation(Summary = "Retorna todas as motos com paginação", Description = "Este endpoint retorna uma lista de motos com suporte a paginação. Utilize os parâmetros 'page' e 'pageSize' para controle da paginação.")]
        [ProducesResponseType(typeof(PaginatedResponse<MotoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<PaginatedResponse<MotoDto>>> GetMotos(int page = 1, int pageSize = 10)
        {
            var motos = await _motoService.GetMotosPaginatedAsync(page, pageSize);

            if (!motos.Data.Any())
                return NoContent(); 

            foreach (var moto in motos.Data)
            {
                moto.Links = new[]
                {
                    new { rel = "self", href = Url.Link(nameof(GetMotoByPlaca), new { placa = moto.Placa }) },
                    new { rel = "update", href = Url.Link(nameof(UpdateMoto), new { placa = moto.Placa }) },
                    new { rel = "delete", href = Url.Link(nameof(DeleteMoto), new { placa = moto.Placa }) }
                };
            }

            return Ok(motos);
        }

        /// <summary>
        /// Retorna uma moto específica pelo número da placa.
        /// </summary>
        /// <param name="placa"> placa da moto.</param>
        /// <returns>Moto encontrada.</returns>
        [HttpGet("{placa}")]
        [SwaggerOperation(Summary = "Retorna uma moto específica", Description = "Este endpoint retorna uma moto com base no número da placa.")]
        [ProducesResponseType(typeof(MotoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Moto>> GetMotoByPlaca(string placa)
        {
            var moto = await _motoService.GetMotoByPlacaAsync(placa);

            if (moto == null)
                return NotFound(new { message = "Moto não encontrada." }); 

            var motoLink = new
            {
                moto.Placa,
                moto.Modelo,
                moto.Status,
                moto.Setor,
                Links = new[]
                {
                    new { rel = "self", href = Url.Link(nameof(GetMotoByPlaca), new { placa = moto.Placa }) },
                    new { rel = "update", href = Url.Link(nameof(UpdateMoto), new { placa = moto.Placa }) },
                    new { rel = "delete", href = Url.Link(nameof(DeleteMoto), new { placa = moto.Placa }) }
                }
            };

            return Ok(motoLink); 
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Cria uma nova moto", Description = "Este endpoint cria uma nova moto, associando-a ao pátio e funcionário existentes.")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Moto>> CreateMoto([FromBody] MotoDto motoDto)
        {
            var result = await _motoService.CreateMotoAsync(motoDto);

            if (result.Contains("sucesso"))
            {
                var moto = await _motoService.GetMotoByPlacaAsync(motoDto.Placa);

                var motoLink = new
                {
                    moto.Placa,
                    moto.Modelo,
                    moto.Status,
                    moto.Setor,
                    Links = new[]
                    {
                        new { rel = "self", href = Url.Link(nameof(GetMotoByPlaca), new { placa = moto.Placa }) },
                        new { rel = "update", href = Url.Link(nameof(UpdateMoto), new { placa = moto.Placa }) },
                        new { rel = "delete", href = Url.Link(nameof(DeleteMoto), new { placa = moto.Placa }) }
                    }
                };

                return CreatedAtAction(nameof(GetMotoByPlaca), new { placa = moto.Placa }, motoLink);
            }

            return BadRequest(new { message = result }); 
        }

        [HttpPut("{placa}")]
        [SwaggerOperation(Summary = "Atualiza uma moto existente", Description = "Este endpoint atualiza as informações de uma moto.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateMoto(string placa, [FromBody] MotoDto motoDto)
        {
            var result = await _motoService.UpdateMotoAsync(placa, motoDto);

            if (result.Contains("atualizada"))
            {
                var moto = await _motoService.GetMotoByPlacaAsync(placa);
                var motoLink = new
                {
                    moto.Placa,
                    moto.Modelo,
                    moto.Status,
                    moto.Setor,
                    Links = new[]
                    {
                        new { rel = "self", href = Url.Link(nameof(GetMotoByPlaca), new { placa = moto.Placa }) },
                        new { rel = "update", href = Url.Link(nameof(UpdateMoto), new { placa = moto.Placa }) },
                        new { rel = "delete", href = Url.Link(nameof(DeleteMoto), new { placa = moto.Placa }) }
                    }
                };
                return Ok(motoLink); 
            }

            return NotFound(new { message = result });
        }


        [HttpDelete("{placa}")]
        [SwaggerOperation(Summary = "Exclui uma moto", Description = "Este endpoint exclui uma moto do sistema.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteMoto(string placa)
        {
            var result = await _motoService.DeleteMotoAsync(placa);

            if (result.Contains("removida"))
                return NoContent(); 

            return NotFound(new { message = result });
        }
    }
}
