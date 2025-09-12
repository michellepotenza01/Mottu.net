using MottuApi.Models;
using MottuApi.DTOs;
using MottuApi.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<ServiceResponse<IEnumerable<Patio>>> GetPatiosAsync()
        {
            var patios = await _patioRepository.GetAllAsync();
            return new ServiceResponse<IEnumerable<Patio>>(patios);
        }

        public async Task<ServiceResponse<Patio>> GetPatioAsync(string nomePatio)
        {
            var patio = await _patioRepository.GetByIdAsync(nomePatio);
            if (patio == null)
                return new ServiceResponse<Patio> { Success = false, Message = "Pátio não encontrado" };

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
            return new ServiceResponse<Patio>(patio, "Pátio criado com sucesso!");
        }

        public async Task<ServiceResponse<Patio>> UpdatePatioAsync(string nomePatio, PatioDto patioDto)
        {
            var patioExistente = await _patioRepository.GetByIdAsync(nomePatio);
            if (patioExistente == null)
                return new ServiceResponse<Patio> { Success = false, Message = "Pátio não encontrado" };

            if (patioDto.VagasTotais < patioExistente.VagasOcupadas)
                return new ServiceResponse<Patio> { Success = false, Message = "Não é possível reduzir vagas totais abaixo das vagas ocupadas" };

            patioExistente.Localizacao = patioDto.Localizacao;
            patioExistente.VagasTotais = patioDto.VagasTotais;

            await _patioRepository.UpdateAsync(patioExistente);
            return new ServiceResponse<Patio>(patioExistente, "Pátio atualizado com sucesso!");
        }

        public async Task<ServiceResponse<bool>> DeletePatioAsync(string nomePatio)
        {
            var patio = await _patioRepository.GetByIdAsync(nomePatio);
            if (patio == null)
                return new ServiceResponse<bool> { Success = false, Message = "Pátio não encontrado" };

            var motosNoPatio = await _motoRepository.GetByPatioAsync(nomePatio);
            if (motosNoPatio.Any())
                return new ServiceResponse<bool> { Success = false, Message = "Não é possível excluir o pátio enquanto houver motos associadas" };

            await _patioRepository.DeleteAsync(patio);
            return new ServiceResponse<bool>(true, "Pátio removido com sucesso!");
        }

        public async Task<ServiceResponse<IEnumerable<Patio>>> GetPatiosPaginatedAsync(int page, int pageSize)
        {
            var patios = await _patioRepository.GetAllAsync();
            var paginated = patios.Skip((page - 1) * pageSize).Take(pageSize);
            return new ServiceResponse<IEnumerable<Patio>>(paginated);
        }

        public async Task<ServiceResponse<int>> GetTotalPatiosCountAsync()
        {
            var patios = await _patioRepository.GetAllAsync();
            return new ServiceResponse<int>(patios.Count());
        }

        public async Task<ServiceResponse<bool>> VerificarVagasDisponiveisAsync(string nomePatio)
        {
            var patio = await _patioRepository.GetByIdAsync(nomePatio);
            if (patio == null)
                return new ServiceResponse<bool> { Success = false, Message = "Pátio não encontrado" };

            return new ServiceResponse<bool>(patio.TemVagasDisponiveis());
        }

        public async Task<ServiceResponse<int>> GetVagasDisponiveisAsync(string nomePatio)
        {
            var patio = await _patioRepository.GetByIdAsync(nomePatio);
            if (patio == null)
                return new ServiceResponse<int> { Success = false, Message = "Pátio não encontrado" };

            return new ServiceResponse<int>(patio.VagasDisponiveis);
        }
    }
}