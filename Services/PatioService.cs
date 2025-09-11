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
        /// Obtém todos os pátios cadastrados.
        /// </summary>
        /// <returns>Lista de pátios.</returns>
        [SwaggerResponse(200, "Pátios encontrados", typeof(IEnumerable<Patio>))]
        public async Task<IEnumerable<Patio>> GetPatiosAsync()
        {
            return await _patioRepository.GetAllAsync();
        }

        /// <summary>
        /// Obtém um pátio específico pelo nome.
        /// </summary>
        /// <param name="nomePatio">Nome do pátio.</param>
        /// <returns>Pátio encontrado.</returns>
        [SwaggerResponse(200, "Pátio encontrado", typeof(Patio))]
        [SwaggerResponse(404, "Pátio não encontrado")]
        public async Task<Patio> GetPatioAsync(string nomePatio)
        {
            return await _patioRepository.GetByIdAsync(nomePatio);
        }

        /// <summary>
        /// Cria um novo pátio.
        /// </summary>
        /// <param name="patioDto">Informações do pátio a ser criado.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        [SwaggerResponse(201, "Pátio criado com sucesso")]
        [SwaggerResponse(400, "Erro ao criar pátio")]
        public async Task<string> CreatePatioAsync(PatioDto patioDto)
        {
            if (patioDto.VagasOcupadas > patioDto.VagasTotais)
                return "O número de vagas ocupadas não pode ser maior que o número total de vagas.";

            var patio = new Patio
            {
                NomePatio = patioDto.NomePatio,
                Localizacao = patioDto.Localizacao,
                VagasTotais = patioDto.VagasTotais,
                VagasOcupadas = patioDto.VagasOcupadas
            };

            await _patioRepository.AddAsync(patio);
            return "Pátio criado com sucesso!";
        }

        /// <summary>
        /// Atualiza as informações de um pátio existente.
        /// </summary>
        /// <param name="nomePatio">Nome do pátio a ser atualizado.</param>
        /// <param name="patioDto">Novas informações do pátio.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        [SwaggerResponse(200, "Pátio atualizado com sucesso")]
        [SwaggerResponse(404, "Pátio não encontrado")]
        public async Task<string> UpdatePatioAsync(string nomePatio, PatioDto patioDto)
        {
            var patioExistente = await _patioRepository.GetByIdAsync(nomePatio);
            if (patioExistente == null)
                return "Pátio não encontrado.";

            patioExistente.Localizacao = patioDto.Localizacao;
            patioExistente.VagasTotais = patioDto.VagasTotais;

            if (patioDto.VagasOcupadas > patioExistente.VagasTotais)
                return "O número de vagas ocupadas não pode ser maior que o número total de vagas.";

            patioExistente.VagasOcupadas = patioDto.VagasOcupadas;

            await _patioRepository.UpdateAsync(patioExistente);
            return "Pátio atualizado com sucesso!";
        }

        /// <summary>
        /// Exclui um pátio do sistema.
        /// </summary>
        /// <param name="nomePatio">Nome do pátio a ser excluído.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        [SwaggerResponse(200, "Pátio removido com sucesso")]
        [SwaggerResponse(404, "Pátio não encontrado")]
        public async Task<string> DeletePatioAsync(string nomePatio)
        {
            var patio = await _patioRepository.GetByIdAsync(nomePatio);
            if (patio == null)
                return "Pátio não encontrado.";

            var motosNoPatio = await _motoRepository.GetByPatioAsync(nomePatio);
            if (motosNoPatio.Any())
                return "Não é possível excluir o pátio enquanto houver motos associadas.";

            await _patioRepository.DeleteAsync(patio);
            return "Pátio removido com sucesso!";
        }
    }
}
