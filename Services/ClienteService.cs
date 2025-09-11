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
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IPatioRepository _patioRepository;

        public ClienteService(IClienteRepository clienteRepository, IPatioRepository patioRepository)
        {
            _clienteRepository = clienteRepository;
            _patioRepository = patioRepository;
        }

        /// <summary>
        /// Obtém todos os clientes cadastrados.
        /// </summary>
        /// <returns>Lista de clientes.</returns>
        [SwaggerResponse(200, "Clientes encontrados", typeof(IEnumerable<Cliente>))]
        public async Task<IEnumerable<Cliente>> GetClientesAsync()
        {
            return await _clienteRepository.GetAllAsync();
        }

        /// <summary>
        /// Obtém um cliente específico pelo nome de usuário.
        /// </summary>
        /// <param name="usuarioCliente">Nome de usuário do cliente.</param>
        /// <returns>Cliente encontrado.</returns>
        [SwaggerResponse(200, "Cliente encontrado", typeof(Cliente))]
        [SwaggerResponse(404, "Cliente não encontrado")]
        public async Task<Cliente> GetClienteByIdAsync(string usuarioCliente)
        {
            return await _clienteRepository.GetByIdAsync(usuarioCliente);
        }

        /// <summary>
        /// Cria um novo cliente.
        /// </summary>
        /// <param name="clienteDto">Informações do cliente.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        [SwaggerResponse(201, "Cliente criado com sucesso")]
        [SwaggerResponse(400, "Erro ao criar cliente")]
        public async Task<string> CreateClienteAsync(ClienteDto clienteDto)
        {
            var cliente = new Cliente
            {
                UsuarioCliente = clienteDto.UsuarioCliente,
                Nome = clienteDto.Nome,
                Senha = clienteDto.Senha,
                MotoPlaca = clienteDto.MotoPlaca
            };

            var patio = await _patioRepository.GetByIdAsync(clienteDto.NomePatio);
            if (patio == null)
                return "Pátio não encontrado.";

            cliente.Patio = patio;
            await _clienteRepository.AddAsync(cliente);
            return "Cliente criado com sucesso!";
        }

        /// <summary>
        /// Atualiza as informações de um cliente.
        /// </summary>
        /// <param name="usuarioCliente">Nome de usuário do cliente.</param>
        /// <param name="clienteDto">Novas informações do cliente.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        [SwaggerResponse(200, "Cliente atualizado com sucesso")]
        [SwaggerResponse(404, "Cliente não encontrado")]
        public async Task<string> UpdateClienteAsync(string usuarioCliente, ClienteDto clienteDto)
        {
            var clienteExistente = await _clienteRepository.GetByIdAsync(usuarioCliente);
            if (clienteExistente == null)
                return "Cliente não encontrado.";

            clienteExistente.Nome = clienteDto.Nome;
            clienteExistente.Senha = clienteDto.Senha;
            clienteExistente.MotoPlaca = clienteDto.MotoPlaca;

            await _clienteRepository.UpdateAsync(clienteExistente);
            return "Cliente atualizado com sucesso!";
        }

        /// <summary>
        /// Exclui um cliente do sistema.
        /// </summary>
        /// <param name="usuarioCliente">Nome de usuário do cliente.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        [SwaggerResponse(200, "Cliente excluído com sucesso")]
        [SwaggerResponse(404, "Cliente não encontrado")]
        public async Task<string> DeleteClienteAsync(string usuarioCliente)
        {
            var cliente = await _clienteRepository.GetByIdAsync(usuarioCliente);
            if (cliente == null)
                return "Cliente não encontrado.";

            await _clienteRepository.DeleteAsync(cliente);
            return "Cliente excluído com sucesso!";
        }
    }
}

