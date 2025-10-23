using MottuApi.Models;
using MottuApi.DTOs;
using MottuApi.Repositories;
using MottuApi.Models.Common;

namespace MottuApi.Services
{
    public class PatioService
    {
        private readonly PatioRepository _patioRepository;
        private readonly MotoRepository _motoRepository;
        private readonly FuncionarioRepository _funcionarioRepository;

        public PatioService(PatioRepository patioRepository, MotoRepository motoRepository, FuncionarioRepository funcionarioRepository)
        {
            _patioRepository = patioRepository;
            _motoRepository = motoRepository;
            _funcionarioRepository = funcionarioRepository;
        }

        public async Task<ServiceResponse<List<Patio>>> GetPatiosAsync()
        {
            try
            {
                var patios = await _patioRepository.GetAllAsync();
                return ServiceResponse<List<Patio>>.Ok(patios, "Pátios recuperados com sucesso");
            }
            catch (Exception ex)
            {
                return ServiceResponse<List<Patio>>.Error($"Erro ao buscar pátios: {ex.Message}");
            }
        }

        public async Task<ServiceResponse<Patio>> GetPatioAsync(string nomePatio)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nomePatio))
                    return ServiceResponse<Patio>.Error("Nome do pátio é obrigatório");

                var patio = await _patioRepository.GetByIdAsync(nomePatio);
                return patio is null 
                    ? ServiceResponse<Patio>.NotFound("Pátio")
                    : ServiceResponse<Patio>.Ok(patio, "Pátio encontrado com sucesso");
            }
            catch (Exception ex)
            {
                return ServiceResponse<Patio>.Error($"Erro ao buscar pátio: {ex.Message}");
            }
        }

        public async Task<ServiceResponse<Patio>> CreatePatioAsync(PatioDto patioDto)
        {
            try
            {
                if (await _patioRepository.ExistsAsync(patioDto.NomePatio))
                    return ServiceResponse<Patio>.Error("Pátio já cadastrado");

                if (patioDto.VagasTotais <= 0)
                    return ServiceResponse<Patio>.Error("Número de vagas deve ser maior que zero");

                var patio = new Patio
                {
                    NomePatio = patioDto.NomePatio.Trim(),
                    Localizacao = patioDto.Localizacao.Trim(),
                    VagasTotais = patioDto.VagasTotais,
                    VagasOcupadas = 0
                };

                await _patioRepository.AddAsync(patio);
                return ServiceResponse<Patio>.Ok(patio, "Pátio criado com sucesso!");
            }
            catch (Exception ex)
            {
                return ServiceResponse<Patio>.Error($"Erro ao criar pátio: {ex.Message}");
            }
        }

        public async Task<ServiceResponse<Patio>> UpdatePatioAsync(string nomePatio, PatioDto patioDto)
        {
            try
            {
                var patioExistente = await _patioRepository.GetByIdAsync(nomePatio);
                if (patioExistente is null)
                    return ServiceResponse<Patio>.NotFound("Pátio");

                if (patioDto.VagasTotais < patioExistente.VagasOcupadas)
                    return ServiceResponse<Patio>.Error("Não é possível reduzir vagas totais abaixo das vagas ocupadas");

                patioExistente.Localizacao = patioDto.Localizacao.Trim();
                patioExistente.VagasTotais = patioDto.VagasTotais;

                await _patioRepository.UpdateAsync(patioExistente);
                return ServiceResponse<Patio>.Ok(patioExistente, "Pátio atualizado com sucesso!");
            }
            catch (Exception ex)
            {
                return ServiceResponse<Patio>.Error($"Erro ao atualizar pátio: {ex.Message}");
            }
        }

        public async Task<ServiceResponse<bool>> DeletePatioAsync(string nomePatio)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nomePatio))
                    return ServiceResponse<bool>.Error("Nome do pátio é obrigatório");

                var patio = await _patioRepository.GetByIdAsync(nomePatio);
                if (patio is null)
                    return ServiceResponse<bool>.NotFound("Pátio");

                var motosNoPatio = await _motoRepository.GetByPatioAsync(nomePatio);
                if (motosNoPatio.Any())
                    return ServiceResponse<bool>.Error("Não é possível excluir o pátio enquanto houver motos associadas");

                var funcionariosNoPatio = await _funcionarioRepository.GetByPatioAsync(nomePatio);
                if (funcionariosNoPatio.Any())
                    return ServiceResponse<bool>.Error("Não é possível excluir o pátio enquanto houver funcionários associados");

                await _patioRepository.DeleteAsync(patio);
                return ServiceResponse<bool>.Ok(true, "Pátio removido com sucesso!");
            }
            catch (Exception ex)
            {
                return ServiceResponse<bool>.Error($"Erro ao excluir pátio: {ex.Message}");
            }
        }

        public async Task<ServiceResponse<bool>> VerificarVagasDisponiveisAsync(string nomePatio)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nomePatio))
                    return ServiceResponse<bool>.Error("Nome do pátio é obrigatório");

                var patio = await _patioRepository.GetByIdAsync(nomePatio);
                if (patio is null)
                    return ServiceResponse<bool>.NotFound("Pátio");

                return ServiceResponse<bool>.Ok(patio.TemVagaDisponivel(), "Verificação de vagas realizada");
            }
            catch (Exception ex)
            {
                return ServiceResponse<bool>.Error($"Erro ao verificar vagas: {ex.Message}");
            }
        }

        public async Task<ServiceResponse<List<Patio>>> GetPatiosComVagasDisponiveisAsync()
        {
            try
            {
                var patios = await _patioRepository.GetPatiosComVagasDisponiveisAsync();
                return ServiceResponse<List<Patio>>.Ok(patios, "Pátios com vagas disponíveis recuperados");
            }
            catch (Exception ex)
            {
                return ServiceResponse<List<Patio>>.Error($"Erro ao buscar pátios com vagas: {ex.Message}");
            }
        }
    }
}