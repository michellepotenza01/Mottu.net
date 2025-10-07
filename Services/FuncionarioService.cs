using MottuApi.Models;
using MottuApi.DTOs;
using MottuApi.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MottuApi.Services
{
    public class FuncionarioService
    {
        private readonly FuncionarioRepository _funcionarioRepository;
        private readonly PatioRepository _patioRepository;

        public FuncionarioService(FuncionarioRepository funcionarioRepository, PatioRepository patioRepository)
        {
            _funcionarioRepository = funcionarioRepository;
            _patioRepository = patioRepository;
        }

        public async Task<ServiceResponse<List<Funcionario>>> GetFuncionariosAsync()
        {
            var funcionarios = await _funcionarioRepository.GetAllAsync();
            return new ServiceResponse<List<Funcionario>>(funcionarios);
        }

        public async Task<ServiceResponse<Funcionario>> GetFuncionarioByIdAsync(string usuarioFuncionario)
        {
            var funcionario = await _funcionarioRepository.GetByIdAsync(usuarioFuncionario);
            if (funcionario == null)
                return new ServiceResponse<Funcionario> { Success = false, Message = "Funcionario nao encontrado" };

            return new ServiceResponse<Funcionario>(funcionario);
        }

        public async Task<ServiceResponse<Funcionario>> CreateFuncionarioAsync(FuncionarioDto funcionarioDto)
        {
            var patio = await _patioRepository.GetByIdAsync(funcionarioDto.NomePatio);
            if (patio == null)
                return new ServiceResponse<Funcionario> { Success = false, Message = "Patio nao encontrado" };

            var funcionario = new Funcionario
            {
                UsuarioFuncionario = funcionarioDto.UsuarioFuncionario,
                Nome = funcionarioDto.Nome,
                Senha = funcionarioDto.Senha,
                NomePatio = funcionarioDto.NomePatio
            };

            await _funcionarioRepository.AddAsync(funcionario);
            return new ServiceResponse<Funcionario>(funcionario, "Funcionario criado com sucesso!");
        }

        public async Task<ServiceResponse<Funcionario>> UpdateFuncionarioAsync(string usuarioFuncionario, FuncionarioDto funcionarioDto)
        {
            var funcionarioExistente = await _funcionarioRepository.GetByIdAsync(usuarioFuncionario);
            if (funcionarioExistente == null)
                return new ServiceResponse<Funcionario> { Success = false, Message = "Funcionario nao encontrado" };

            var patio = await _patioRepository.GetByIdAsync(funcionarioDto.NomePatio);
            if (patio == null)
                return new ServiceResponse<Funcionario> { Success = false, Message = "Patio nao encontrado" };

            funcionarioExistente.Nome = funcionarioDto.Nome;
            funcionarioExistente.Senha = funcionarioDto.Senha;
            funcionarioExistente.NomePatio = funcionarioDto.NomePatio;

            await _funcionarioRepository.UpdateAsync(funcionarioExistente);
            return new ServiceResponse<Funcionario>(funcionarioExistente, "Funcionario atualizado com sucesso!");
        }

        public async Task<ServiceResponse<bool>> DeleteFuncionarioAsync(string usuarioFuncionario)
        {
            var funcionario = await _funcionarioRepository.GetByIdAsync(usuarioFuncionario);
            if (funcionario == null)
                return new ServiceResponse<bool> { Success = false, Message = "Funcionario nao encontrado" };

            await _funcionarioRepository.DeleteAsync(funcionario);
            return new ServiceResponse<bool>(true, "Funcionario excluido com sucesso!");
        }

        public async Task<ServiceResponse<List<Funcionario>>> GetFuncionariosPaginatedAsync(int page, int pageSize)
        {
            var funcionarios = await _funcionarioRepository.GetPaginatedAsync(page, pageSize);
            return new ServiceResponse<List<Funcionario>>(funcionarios);
        }

        public async Task<ServiceResponse<int>> GetTotalFuncionariosCountAsync()
        {
            var count = await _funcionarioRepository.GetTotalCountAsync();
            return new ServiceResponse<int>(count);
        }
    }
}