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
        /// Obt�m todos os funcion�rios cadastrados.
        /// </summary>
        /// <returns>Lista de funcion�rios.</returns>
        [SwaggerResponse(200, "Funcion�rios encontrados", typeof(IEnumerable<Funcionario>))]
        public async Task<IEnumerable<Funcionario>> GetFuncionariosAsync()
        {
            return await _funcionarioRepository.GetAllAsync();
        }

        /// <summary>
        /// Obt�m um funcion�rio espec�fico pelo nome de usu�rio.
        /// </summary>
        /// <param name="usuarioFuncionario">Nome de usu�rio do funcion�rio.</param>
        /// <returns>Funcion�rio encontrado.</returns>
        [SwaggerResponse(200, "Funcion�rio encontrado", typeof(Funcionario))]
        [SwaggerResponse(404, "Funcion�rio n�o encontrado")]
        public async Task<Funcionario> GetFuncionarioByIdAsync(string usuarioFuncionario)
        {
            return await _funcionarioRepository.GetByIdAsync(usuarioFuncionario);
        }

        /// <summary>
        /// Cria um novo funcion�rio.
        /// </summary>
        /// <param name="funcionarioDto">Informa��es do funcion�rio.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        [SwaggerResponse(201, "Funcion�rio criado com sucesso")]
        [SwaggerResponse(400, "Erro ao criar funcion�rio")]
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
                return "P�tio n�o encontrado.";

            funcionario.Patio = patio;
            await _funcionarioRepository.AddAsync(funcionario);
            return "Funcion�rio criado com sucesso!";
        }

        /// <summary>
        /// Atualiza as informa��es de um funcion�rio.
        /// </summary>
        /// <param name="usuarioFuncionario">Nome de usu�rio do funcion�rio.</param>
        /// <param name="funcionarioDto">Novas informa��es do funcion�rio.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        [SwaggerResponse(200, "Funcion�rio atualizado com sucesso")]
        [SwaggerResponse(404, "Funcion�rio n�o encontrado")]
        public async Task<string> UpdateFuncionarioAsync(string usuarioFuncionario, FuncionarioDto funcionarioDto)
        {
            var funcionarioExistente = await _funcionarioRepository.GetByIdAsync(usuarioFuncionario);
            if (funcionarioExistente == null)
                return "Funcion�rio n�o encontrado.";

            funcionarioExistente.Nome = funcionarioDto.Nome;
            funcionarioExistente.Senha = funcionarioDto.Senha;

            var patio = await _patioRepository.GetByIdAsync(funcionarioDto.NomePatio);
            if (patio == null)
                return "P�tio n�o encontrado.";

            funcionarioExistente.Patio = patio;
            await _funcionarioRepository.UpdateAsync(funcionarioExistente);
            return "Funcion�rio atualizado com sucesso!";
        }

        /// <summary>
        /// Exclui um funcion�rio do sistema.
        /// </summary>
        /// <param name="usuarioFuncionario">Nome de usu�rio do funcion�rio.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        [SwaggerResponse(200, "Funcion�rio exclu�do com sucesso")]
        [SwaggerResponse(404, "Funcion�rio n�o encontrado")]
        public async Task<string> DeleteFuncionarioAsync(string usuarioFuncionario)
        {
            var funcionario = await _funcionarioRepository.GetByIdAsync(usuarioFuncionario);
            if (funcionario == null)
                return "Funcion�rio n�o encontrado.";

            await _funcionarioRepository.DeleteAsync(funcionario);
            return "Funcion�rio exclu�do com sucesso!";
        }
    }
}
