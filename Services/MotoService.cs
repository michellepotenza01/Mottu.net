using MottuApi.Models;
using MottuApi.DTOs;
using MottuApi.Repositories;
using MottuApi.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MottuApi.Services
{
    public class MotoService : IMotoService
    {
        private readonly IMotoRepository _motoRepository;
        private readonly IFuncionarioRepository _funcionarioRepository;
        private readonly IPatioRepository _patioRepository;

        public MotoService(IMotoRepository motoRepository, IFuncionarioRepository funcionarioRepository, IPatioRepository patioRepository)
        {
            _motoRepository = motoRepository;
            _funcionarioRepository = funcionarioRepository;
            _patioRepository = patioRepository;
        }

        public async Task<ServiceResponse<IEnumerable<Moto>>> GetMotosAsync(StatusMoto? status = null, SetorMoto? setor = null)
        {
            var motos = await _motoRepository.GetAllAsync();

            if (status.HasValue)
                motos = motos.Where(m => m.Status == status.Value);

            if (setor.HasValue)
                motos = motos.Where(m => m.Setor == setor.Value);

            return new ServiceResponse<IEnumerable<Moto>>(motos);
        }

        public async Task<ServiceResponse<Moto>> GetMotoAsync(string placa)
        {
            var moto = await _motoRepository.GetByIdAsync(placa);
            if (moto == null)
                return new ServiceResponse<Moto> { Success = false, Message = "Moto não encontrada" };

            return new ServiceResponse<Moto>(moto);
        }

        public async Task<ServiceResponse<Moto>> CreateMotoAsync(MotoDto motoDto)
        {
            var funcionario = await _funcionarioRepository.GetByIdAsync(motoDto.UsuarioFuncionario);
            if (funcionario == null)
                return new ServiceResponse<Moto> { Success = false, Message = "Funcionário não encontrado" };

            var patio = await _patioRepository.GetByIdAsync(motoDto.NomePatio);
            if (patio == null)
                return new ServiceResponse<Moto> { Success = false, Message = "Pátio não encontrado" };

            if (!patio.TemVagasDisponiveis())
                return new ServiceResponse<Moto> { Success = false, Message = "Não há vagas disponíveis no pátio" };

            var moto = new Moto
            {
                Placa = motoDto.Placa,
                Modelo = motoDto.Modelo,
                Status = motoDto.Status,
                Setor = motoDto.Setor,
                NomePatio = motoDto.NomePatio,
                UsuarioFuncionario = motoDto.UsuarioFuncionario,
                Funcionario = funcionario,
                Patio = patio
            };

            // LÓGICA DE VAGAS ✅
            if (moto.Status == StatusMoto.Disponível || moto.Status == StatusMoto.Manutenção)
            {
                patio.VagasOcupadas++;
                await _patioRepository.UpdateAsync(patio);
            }

            await _motoRepository.AddAsync(moto);
            return new ServiceResponse<Moto>(moto, "Moto criada com sucesso!");
        }

        public async Task<ServiceResponse<Moto>> UpdateMotoAsync(string placa, MotoDto motoDto)
        {
            var motoExistente = await _motoRepository.GetByIdAsync(placa);
            if (motoExistente == null)
                return new ServiceResponse<Moto> { Success = false, Message = "Moto não encontrada" };

            var patioAntigo = motoExistente.Patio;
            var statusAntigo = motoExistente.Status;

            motoExistente.Modelo = motoDto.Modelo;
            motoExistente.Status = motoDto.Status;
            motoExistente.Setor = motoDto.Setor;

            // LÓGICA DE VAGAS ✅
            if (statusAntigo != motoDto.Status)
            {
                if ((statusAntigo == StatusMoto.Disponível || statusAntigo == StatusMoto.Manutenção) &&
                    motoDto.Status == StatusMoto.Alugada)
                {
                    patioAntigo.VagasOcupadas--;
                    await _patioRepository.UpdateAsync(patioAntigo);
                }
                else if (statusAntigo == StatusMoto.Alugada &&
                         (motoDto.Status == StatusMoto.Disponível || motoDto.Status == StatusMoto.Manutenção))
                {
                    patioAntigo.VagasOcupadas++;
                    await _patioRepository.UpdateAsync(patioAntigo);
                }
            }

            await _motoRepository.UpdateAsync(motoExistente);
            return new ServiceResponse<Moto>(motoExistente, "Moto atualizada com sucesso!");
        }

        public async Task<ServiceResponse<bool>> DeleteMotoAsync(string placa)
        {
            var moto = await _motoRepository.GetByIdAsync(placa);
            if (moto == null)
                return new ServiceResponse<bool> { Success = false, Message = "Moto não encontrada" };

            var patio = moto.Patio;

            if (moto.Status == StatusMoto.Disponível || moto.Status == StatusMoto.Manutenção)
            {
                patio.VagasOcupadas--;
                await _patioRepository.UpdateAsync(patio);
            }

            await _motoRepository.DeleteAsync(moto);
            return new ServiceResponse<bool>(true, "Moto removida com sucesso!");
        }

        // Métodos de paginação
        public async Task<ServiceResponse<IEnumerable<Moto>>> GetMotosPaginatedAsync(int page, int pageSize, StatusMoto? status = null, SetorMoto? setor = null)
        {
            var motos = await _motoRepository.GetAllAsync();

            if (status.HasValue)
                motos = motos.Where(m => m.Status == status.Value);

            if (setor.HasValue)
                motos = motos.Where(m => m.Setor == setor.Value);

            var paginated = motos.Skip((page - 1) * pageSize).Take(pageSize);
            return new ServiceResponse<IEnumerable<Moto>>(paginated);
        }

        public async Task<ServiceResponse<int>> GetTotalMotosCountAsync(StatusMoto? status = null, SetorMoto? setor = null)
        {
            var motos = await _motoRepository.GetAllAsync();

            if (status.HasValue)
                motos = motos.Where(m => m.Status == status.Value);

            if (setor.HasValue)
                motos = motos.Where(m => m.Setor == setor.Value);

            return new ServiceResponse<int>(motos.Count());
        }

        public async Task<ServiceResponse<IEnumerable<Moto>>> GetMotosByPatioAsync(string nomePatio)
        {
            var motos = await _motoRepository.GetByPatioAsync(nomePatio);
            return new ServiceResponse<IEnumerable<Moto>>(motos);
        }
    }
}