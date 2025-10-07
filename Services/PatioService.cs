using MottuApi.Models;
using MottuApi.DTOs;
using MottuApi.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MottuApi.Services
{
    public class PatioService
    {
        private readonly PatioRepository _patioRepository;
        private readonly MotoRepository _motoRepository;

        public PatioService(PatioRepository patioRepository, MotoRepository motoRepository)
        {
            _patioRepository = patioRepository;
            _motoRepository = motoRepository;
        }

        public async Task<ServiceResponse<List<Patio>>> GetPatiosAsync()
        {
            var patios = await _patioRepository.GetAllAsync();
            return new ServiceResponse<List<Patio>>(patios);
        }

        public async Task<ServiceResponse<Patio>> GetPatioAsync(string nomePatio)
        {
            var patio = await _patioRepository.GetByIdAsync(nomePatio);
            if (patio == null)
                return new ServiceResponse<Patio> { Success = false, Message = "Patio nao encontrado" };

            return new ServiceResponse<Patio>(patio);
        }

        public async Task<ServiceResponse<Patio>> CreatePatioAsync(PatioDto patioDto)
        {
            var patio = new Patio
            {
                NomePatio = patioDto.NomePatio,
                Localizacao = patioDto.Localizacao,
                VagasTotais = patioDto.VagasTotais,
                VagasOcupadas = 0
            };

            await _patioRepository.AddAsync(patio);
            return new ServiceResponse<Patio>(patio, "Patio criado com sucesso!");
        }

        public async Task<ServiceResponse<Patio>> UpdatePatioAsync(string nomePatio, PatioDto patioDto)
        {
            var patioExistente = await _patioRepository.GetByIdAsync(nomePatio);
            if (patioExistente == null)
                return new ServiceResponse<Patio> { Success = false, Message = "Patio nao encontrado" };

            if (patioDto.VagasTotais < patioExistente.VagasOcupadas)
                return new ServiceResponse<Patio> { Success = false, Message = "Nao foi possivel reduzir vagas totais abaixo das vagas ocupadas" };

            patioExistente.Localizacao = patioDto.Localizacao;
            patioExistente.VagasTotais = patioDto.VagasTotais;

            await _patioRepository.UpdateAsync(patioExistente);
            return new ServiceResponse<Patio>(patioExistente, "P�tio atualizado com sucesso!");
        }

        public async Task<ServiceResponse<bool>> DeletePatioAsync(string nomePatio)
        {
            var patio = await _patioRepository.GetByIdAsync(nomePatio);
            if (patio == null)
                return new ServiceResponse<bool> { Success = false, Message = "Patio nao encontrado" };

            var motosNoPatio = await _motoRepository.GetByPatioAsync(nomePatio);
            if (motosNoPatio.Any())
                return new ServiceResponse<bool> { Success = false, Message = "Nao foi possivel excluir o patio enquanto houver motos associadas" };

            await _patioRepository.DeleteAsync(patio);
            return new ServiceResponse<bool>(true, "Patio removido com sucesso!");
        }

        public async Task<ServiceResponse<List<Patio>>> GetPatiosPaginatedAsync(int page, int pageSize)
        {
            var patios = await _patioRepository.GetPaginatedAsync(page, pageSize);
            return new ServiceResponse<List<Patio>>(patios);
        }

        public async Task<ServiceResponse<int>> GetTotalPatiosCountAsync()
        {
            var count = await _patioRepository.GetTotalCountAsync();
            return new ServiceResponse<int>(count);
        }

        public async Task<ServiceResponse<bool>> VerificarVagasDisponiveisAsync(string nomePatio)
        {
            var patio = await _patioRepository.GetByIdAsync(nomePatio);
            if (patio == null)
                return new ServiceResponse<bool> { Success = false, Message = "P�tio n�o encontrado" };

            return new ServiceResponse<bool>(patio.VagasDisponiveis > 0);
        }

        public async Task<ServiceResponse<int>> GetVagasDisponiveisAsync(string nomePatio)
        {
            var patio = await _patioRepository.GetByIdAsync(nomePatio);
            if (patio == null)
                return new ServiceResponse<int> { Success = false, Message = "P�tio n�o encontrado" };

            return new ServiceResponse<int>(patio.VagasDisponiveis);
        }
    }
}