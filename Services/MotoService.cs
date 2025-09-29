using MottuApi.Models;
using MottuApi.DTOs;
using MottuApi.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MottuApi3.Enums;

namespace MottuApi.Services
{
    public class MotoService
    {
        private readonly MotoRepository _motoRepository;
        private readonly FuncionarioRepository _funcionarioRepository;
        private readonly PatioRepository _patioRepository;

        public MotoService(MotoRepository motoRepository, FuncionarioRepository funcionarioRepository, PatioRepository patioRepository)
        {
            _motoRepository = motoRepository;
            _funcionarioRepository = funcionarioRepository;
            _patioRepository = patioRepository;
        }

        public async Task<ServiceResponse<List<Moto>>> GetMotosAsync(StatusMoto? status = null, SetorMoto? setor = null)
        {
            var motos = await _motoRepository.GetAllAsync();

            if (status.HasValue)
                motos = motos.Where(m => m.Status == status.Value).ToList();

            if (setor.HasValue)
                motos = motos.Where(m => m.Setor == setor.Value).ToList();

            return new ServiceResponse<List<Moto>>(motos);
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
            var motoExistente = await _motoRepository.GetByIdAsync(motoDto.Placa);
            if (motoExistente != null)
                return new ServiceResponse<Moto> { Success = false, Message = "Placa já cadastrada" };

            var funcionario = await _funcionarioRepository.GetByIdAsync(motoDto.UsuarioFuncionario);
            if (funcionario == null)
                return new ServiceResponse<Moto> { Success = false, Message = "Funcionário não encontrado" };

            if (funcionario.NomePatio != motoDto.NomePatio)
                return new ServiceResponse<Moto> { Success = false, Message = "Funcionário não pertence a este pátio" };

            var patio = await _patioRepository.GetByIdAsync(motoDto.NomePatio);
            if (patio == null)
                return new ServiceResponse<Moto> { Success = false, Message = "Pátio não encontrado" };

            if (patio.VagasOcupadas >= patio.VagasTotais)
                return new ServiceResponse<Moto> { Success = false, Message = "Não há vagas disponíveis no pátio" };

            var moto = new Moto
            {
                Placa = motoDto.Placa,
                Modelo = motoDto.Modelo,
                Status = motoDto.Status,
                Setor = motoDto.Setor,
                NomePatio = motoDto.NomePatio,
                UsuarioFuncionario = motoDto.UsuarioFuncionario
            };

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

            var funcionario = await _funcionarioRepository.GetByIdAsync(motoDto.UsuarioFuncionario);
            if (funcionario == null)
                return new ServiceResponse<Moto> { Success = false, Message = "Funcionário não encontrado" };

            if (funcionario.NomePatio != motoDto.NomePatio)
                return new ServiceResponse<Moto> { Success = false, Message = "Funcionário não pertence a este pátio" };

            var patioAntigo = await _patioRepository.GetByIdAsync(motoExistente.NomePatio);
            var statusAntigo = motoExistente.Status;

            if (motoExistente.NomePatio != motoDto.NomePatio)
            {
                var novoPatio = await _patioRepository.GetByIdAsync(motoDto.NomePatio);
                if (novoPatio == null)
                    return new ServiceResponse<Moto> { Success = false, Message = "Novo pátio não encontrado" };

                if (novoPatio.VagasOcupadas >= novoPatio.VagasTotais &&
                    (motoDto.Status == StatusMoto.Disponível || motoDto.Status == StatusMoto.Manutenção))
                    return new ServiceResponse<Moto> { Success = false, Message = "Não há vagas disponíveis no novo pátio" };

                if (statusAntigo == StatusMoto.Disponível || statusAntigo == StatusMoto.Manutenção)
                {
                    patioAntigo.VagasOcupadas--;
                    await _patioRepository.UpdateAsync(patioAntigo);
                }

                if (motoDto.Status == StatusMoto.Disponível || motoDto.Status == StatusMoto.Manutenção)
                {
                    novoPatio.VagasOcupadas++;
                    await _patioRepository.UpdateAsync(novoPatio);
                }

                motoExistente.NomePatio = motoDto.NomePatio;
            }
            else
            {
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
                        if (patioAntigo.VagasOcupadas >= patioAntigo.VagasTotais)
                            return new ServiceResponse<Moto> { Success = false, Message = "Não há vagas disponíveis no pátio" };

                        patioAntigo.VagasOcupadas++;
                        await _patioRepository.UpdateAsync(patioAntigo);
                    }
                }
            }

            motoExistente.Modelo = motoDto.Modelo;
            motoExistente.Status = motoDto.Status;
            motoExistente.Setor = motoDto.Setor;
            motoExistente.UsuarioFuncionario = motoDto.UsuarioFuncionario;

            await _motoRepository.UpdateAsync(motoExistente);
            return new ServiceResponse<Moto>(motoExistente, "Moto atualizada com sucesso!");
        }

        public async Task<ServiceResponse<bool>> DeleteMotoAsync(string placa)
        {
            var moto = await _motoRepository.GetByIdAsync(placa);
            if (moto == null)
                return new ServiceResponse<bool> { Success = false, Message = "Moto não encontrada" };

            var patio = await _patioRepository.GetByIdAsync(moto.NomePatio);

            if (moto.Status == StatusMoto.Disponível || moto.Status == StatusMoto.Manutenção)
            {
                patio.VagasOcupadas--;
                await _patioRepository.UpdateAsync(patio);
            }

            await _motoRepository.DeleteAsync(moto);
            return new ServiceResponse<bool>(true, "Moto removida com sucesso!");
        }

        public async Task<ServiceResponse<List<Moto>>> GetMotosPaginatedAsync(int page, int pageSize, StatusMoto? status = null, SetorMoto? setor = null)
        {
            var motos = await _motoRepository.GetAllAsync();

            if (status.HasValue)
                motos = motos.Where(m => m.Status == status.Value).ToList();

            if (setor.HasValue)
                motos = motos.Where(m => m.Setor == setor.Value).ToList();

            var paginated = motos.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return new ServiceResponse<List<Moto>>(paginated);
        }

        public async Task<ServiceResponse<int>> GetTotalMotosCountAsync(StatusMoto? status = null, SetorMoto? setor = null)
        {
            var motos = await _motoRepository.GetAllAsync();

            if (status.HasValue)
                motos = motos.Where(m => m.Status == status.Value).ToList();

            if (setor.HasValue)
                motos = motos.Where(m => m.Setor == setor.Value).ToList();

            return new ServiceResponse<int>(motos.Count);
        }

        public async Task<ServiceResponse<List<Moto>>> GetMotosByPatioAsync(string nomePatio)
        {
            var motos = await _motoRepository.GetByPatioAsync(nomePatio);
            return new ServiceResponse<List<Moto>>(motos);
        }

        public async Task<ServiceResponse<int>> CountMotosByStatusAsync(StatusMoto status, string nomePatio)
        {
            var count = await _motoRepository.CountByStatusAndPatioAsync(status, nomePatio);
            return new ServiceResponse<int>(count);
        }
    }
}