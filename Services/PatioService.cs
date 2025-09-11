using MottuApi.Data;
using MottuApi.Models;
using MottuApi.DTOs;
using MottuApi.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace MottuApi.Services
{
    public class PatioService : IPatioService
    {
        private readonly IPatioRepository _patioRepository;
        private readonly IMotoRepository _motoRepository;

        public PatioService(IPatioRepository patioRepository, IMotoRepository motoRepository)
        {
            _patioRepository = patioRepository;
            _motoRepository = motoRepository;
        }

        /// <summary>
        /// Obt�m todos os p�tios cadastrados.
        /// </summary>
        /// <returns>Lista de p�tios.</returns>
        [SwaggerResponse(200, "P�tios encontrados", typeof(IEnumerable<Patio>))]
        public async Task<IEnumerable<Patio>> GetPatiosAsync()
        {
            return await _patioRepository.GetAllAsync();
        }

        /// <summary>
        /// Obt�m um p�tio espec�fico pelo nome.
        /// </summary>
        /// <param name="nomePatio">Nome do p�tio.</param>
        /// <returns>P�tio encontrado.</returns>
        [SwaggerResponse(200, "P�tio encontrado", typeof(Patio))]
        [SwaggerResponse(404, "P�tio n�o encontrado")]
        public async Task<Patio> GetPatioAsync(string nomePatio)
        {
            return await _patioRepository.GetByIdAsync(nomePatio);
        }

        /// <summary>
        /// Cria um novo p�tio.
        /// </summary>
        /// <param name="patioDto">Informa��es do p�tio a ser criado.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        [SwaggerResponse(201, "P�tio criado com sucesso")]
        [SwaggerResponse(400, "Erro ao criar p�tio")]
        public async Task<string> CreatePatioAsync(PatioDto patioDto)
        {
            if (patioDto.VagasOcupadas > patioDto.VagasTotais)
                return "O n�mero de vagas ocupadas n�o pode ser maior que o n�mero total de vagas.";

            var patio = new Patio
            {
                NomePatio = patioDto.NomePatio,
                Localizacao = patioDto.Localizacao,
                VagasTotais = patioDto.VagasTotais,
                VagasOcupadas = patioDto.VagasOcupadas
            };

            await _patioRepository.AddAsync(patio);
            return "P�tio criado com sucesso!";
        }

        /// <summary>
        /// Atualiza as informa��es de um p�tio existente.
        /// </summary>
        /// <param name="nomePatio">Nome do p�tio a ser atualizado.</param>
        /// <param name="patioDto">Novas informa��es do p�tio.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        [SwaggerResponse(200, "P�tio atualizado com sucesso")]
        [SwaggerResponse(404, "P�tio n�o encontrado")]
        public async Task<string> UpdatePatioAsync(string nomePatio, PatioDto patioDto)
        {
            var patioExistente = await _patioRepository.GetByIdAsync(nomePatio);
            if (patioExistente == null)
                return "P�tio n�o encontrado.";

            patioExistente.Localizacao = patioDto.Localizacao;
            patioExistente.VagasTotais = patioDto.VagasTotais;

            if (patioDto.VagasOcupadas > patioExistente.VagasTotais)
                return "O n�mero de vagas ocupadas n�o pode ser maior que o n�mero total de vagas.";

            patioExistente.VagasOcupadas = patioDto.VagasOcupadas;

            await _patioRepository.UpdateAsync(patioExistente);
            return "P�tio atualizado com sucesso!";
        }

        /// <summary>
        /// Exclui um p�tio do sistema.
        /// </summary>
        /// <param name="nomePatio">Nome do p�tio a ser exclu�do.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        [SwaggerResponse(200, "P�tio removido com sucesso")]
        [SwaggerResponse(404, "P�tio n�o encontrado")]
        public async Task<string> DeletePatioAsync(string nomePatio)
        {
            var patio = await _patioRepository.GetByIdAsync(nomePatio);
            if (patio == null)
                return "P�tio n�o encontrado.";

            var motosNoPatio = await _motoRepository.GetByPatioAsync(nomePatio);
            if (motosNoPatio.Any())
                return "N�o � poss�vel excluir o p�tio enquanto houver motos associadas.";

            await _patioRepository.DeleteAsync(patio);
            return "P�tio removido com sucesso!";
        }
    }
}
