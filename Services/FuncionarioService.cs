using MottuApi.Models;
using MottuApi.DTOs;
using MottuApi.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MottuApi.Services
{
    public class FuncionarioService : IFuncionarioService
    {
        private readonly IFuncionarioRepository _funcionarioRepository;
        private readonly IPatioRepository _patioRepository;

        public FuncionarioService(IFuncionarioRepository funcionarioRepository, IPatioRepository patioRepository)
        {
            _funcionarioRepository = funcionarioRepository;
            _patioRepository = patioRepository;
        }

        public async Task<ServiceResponse<IEnumerable<Funcionario>>> GetFuncionariosAsync()
        {
            var funcionarios = await _funcionarioRepository.GetAllAsync();
            return new ServiceResponse<IEnumerable<Funcionario>>(funcionarios);
        }

        public async Task<ServiceResponse<Funcionario>> GetFuncionarioByIdAsync(string usuarioFuncionario)
        {
            var funcionario = await _funcionarioRepository.GetByIdAsync(usuarioFuncionario);
            if (funcionario == null)
                return new ServiceResponse<Funcionario> { Success = false, Message = "Funcionário não encontrado" };

            return new ServiceResponse<Funcionario>(funcionario);
        }

        public async Task<ServiceResponse<Funcionario>> CreateFuncionarioAsync(FuncionarioDto funcionarioDto)
        {
            var patio = await _patioRepository.GetByIdAsync(funcionarioDto.NomePatio);
            if (patio == null)
                return new ServiceResponse<Funcionario> { Success = false, Message = "Pátio não encontrado" };

            var funcionario = new Funcionario
            {
                UsuarioFuncionario = funcionarioDto.UsuarioFuncionario,
                Nome = funcionarioDto.Nome,
                Senha = funcionarioDto.Senha,
                NomePatio = funcionarioDto.NomePatio,
                Patio = patio
            };

            await _funcionarioRepository.AddAsync(funcionario);
            return new ServiceResponse<Funcionario>(funcionario, "Funcionário criado com sucesso!");
        }

        public async Task<ServiceResponse<Funcionario>> UpdateFuncionarioAsync(string usuarioFuncionario, FuncionarioDto funcionarioDto)
        {
            var funcionarioExistente = await _funcionarioRepository.GetByIdAsync(usuarioFuncionario);
            if (funcionarioExistente == null)
                return new ServiceResponse<Funcionario> { Success = false, Message = "Funcionário não encontrado" };

            var patio = await _patioRepository.GetByIdAsync(funcionarioDto.NomePatio);
            if (patio == null)
                return new ServiceResponse<Funcionario> { Success = false, Message = "Pátio não encontrado" };

            funcionarioExistente.Nome = funcionarioDto.Nome;
            funcionarioExistente.Senha = funcionarioDto.Senha;
            funcionarioExistente.NomePatio = funcionarioDto.NomePatio;
            funcionarioExistente.Patio = patio;

            await _funcionarioRepository.UpdateAsync(funcionarioExistente);
            return new ServiceResponse<Funcionario>(funcionarioExistente, "Funcionário atualizado com sucesso!");
        }

        public async Task<ServiceResponse<bool>> DeleteFuncionarioAsync(string usuarioFuncionario)
        {
            var funcionario = await _funcionarioRepository.GetByIdAsync(usuarioFuncionario);
            if (funcionario == null)
                return new ServiceResponse<bool> { Success = false, Message = "Funcionário não encontrado" };

            await _funcionarioRepository.DeleteAsync(funcionario);
            return new ServiceResponse<bool>(true, "Funcionário excluído com sucesso!");
        }

        public async Task<ServiceResponse<IEnumerable<Funcionario>>> GetFuncionariosPaginatedAsync(int page, int pageSize)
        {
            var funcionarios = await _funcionarioRepository.GetAllAsync();
            var paginated = funcionarios.Skip((page - 1) * pageSize).Take(pageSize);
            return new ServiceResponse<IEnumerable<Funcionario>>(paginated);
        }

        public async Task<ServiceResponse<int>> GetTotalFuncionariosCountAsync()
        {
            var funcionarios = await _funcionarioRepository.GetAllAsync();
            return new ServiceResponse<int>(funcionarios.Count());
        }
    }
}