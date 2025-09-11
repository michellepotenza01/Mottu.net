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
    public class FuncionarioService : IFuncionarioService
    {
        private readonly IFuncionarioRepository _funcionarioRepository;
        private readonly IPatioRepository _patioRepository;

        public FuncionarioService(IFuncionarioRepository funcionarioRepository, IPatioRepository patioRepository)
        {
            _funcionarioRepository = funcionarioRepository;
            _patioRepository = patioRepository;
        }

        /// <summary>
        /// Obtém todos os funcionários cadastrados.
        /// </summary>
        /// <returns>Lista de funcionários.</returns>
        [SwaggerResponse(200, "Funcionários encontrados", typeof(IEnumerable<Funcionario>))]
        public async Task<IEnumerable<Funcionario>> GetFuncionariosAsync()
        {
            return await _funcionarioRepository.GetAllAsync();
        }

        /// <summary>
        /// Obtém um funcionário específico pelo nome de usuário.
        /// </summary>
        /// <param name="usuarioFuncionario">Nome de usuário do funcionário.</param>
        /// <returns>Funcionário encontrado.</returns>
        [SwaggerResponse(200, "Funcionário encontrado", typeof(Funcionario))]
        [SwaggerResponse(404, "Funcionário não encontrado")]
        public async Task<Funcionario> GetFuncionarioByIdAsync(string usuarioFuncionario)
        {
            return await _funcionarioRepository.GetByIdAsync(usuarioFuncionario);
        }

        /// <summary>
        /// Cria um novo funcionário.
        /// </summary>
        /// <param name="funcionarioDto">Informações do funcionário.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        [SwaggerResponse(201, "Funcionário criado com sucesso")]
        [SwaggerResponse(400, "Erro ao criar funcionário")]
        public async Task<string> CreateFuncionarioAsync(FuncionarioDto funcionarioDto)
        {
            var funcionario = new Funcionario
            {
                UsuarioFuncionario = funcionarioDto.UsuarioFuncionario,
                Nome = funcionarioDto.Nome,
                Senha = funcionarioDto.Senha,
                NomePatio = funcionarioDto.NomePatio
            };

            var patio = await _patioRepository.GetByIdAsync(funcionarioDto.NomePatio);
            if (patio == null)
                return "Pátio não encontrado.";

            funcionario.Patio = patio;
            await _funcionarioRepository.AddAsync(funcionario);
            return "Funcionário criado com sucesso!";
        }

        /// <summary>
        /// Atualiza as informações de um funcionário.
        /// </summary>
        /// <param name="usuarioFuncionario">Nome de usuário do funcionário.</param>
        /// <param name="funcionarioDto">Novas informações do funcionário.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        [SwaggerResponse(200, "Funcionário atualizado com sucesso")]
        [SwaggerResponse(404, "Funcionário não encontrado")]
        public async Task<string> UpdateFuncionarioAsync(string usuarioFuncionario, FuncionarioDto funcionarioDto)
        {
            var funcionarioExistente = await _funcionarioRepository.GetByIdAsync(usuarioFuncionario);
            if (funcionarioExistente == null)
                return "Funcionário não encontrado.";

            funcionarioExistente.Nome = funcionarioDto.Nome;
            funcionarioExistente.Senha = funcionarioDto.Senha;

            var patio = await _patioRepository.GetByIdAsync(funcionarioDto.NomePatio);
            if (patio == null)
                return "Pátio não encontrado.";

            funcionarioExistente.Patio = patio;
            await _funcionarioRepository.UpdateAsync(funcionarioExistente);
            return "Funcionário atualizado com sucesso!";
        }

        /// <summary>
        /// Exclui um funcionário do sistema.
        /// </summary>
        /// <param name="usuarioFuncionario">Nome de usuário do funcionário.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        [SwaggerResponse(200, "Funcionário excluído com sucesso")]
        [SwaggerResponse(404, "Funcionário não encontrado")]
        public async Task<string> DeleteFuncionarioAsync(string usuarioFuncionario)
        {
            var funcionario = await _funcionarioRepository.GetByIdAsync(usuarioFuncionario);
            if (funcionario == null)
                return "Funcionário não encontrado.";

            await _funcionarioRepository.DeleteAsync(funcionario);
            return "Funcionário excluído com sucesso!";
        }
    }
}
