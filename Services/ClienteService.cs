using MottuApi.Models;
using MottuApi.DTOs;
using MottuApi.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MottuApi.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IMotoRepository _motoRepository;

        public ClienteService(IClienteRepository clienteRepository, IMotoRepository motoRepository)
        {
            _clienteRepository = clienteRepository;
            _motoRepository = motoRepository;
        }

        public async Task<ServiceResponse<IEnumerable<Cliente>>> GetClientesAsync()
        {
            var clientes = await _clienteRepository.GetAllAsync();
            return new ServiceResponse<IEnumerable<Cliente>>(clientes);
        }

        public async Task<ServiceResponse<Cliente>> GetClienteByIdAsync(string usuarioCliente)
        {
            var cliente = await _clienteRepository.GetByIdAsync(usuarioCliente);
            if (cliente == null)
                return new ServiceResponse<Cliente> { Success = false, Message = "Cliente n�o encontrado" };

            return new ServiceResponse<Cliente>(cliente);
        }

        public async Task<ServiceResponse<Cliente>> CreateClienteAsync(ClienteDto clienteDto)
        {
            if (!string.IsNullOrEmpty(clienteDto.MotoPlaca))
            {
                var moto = await _motoRepository.GetByIdAsync(clienteDto.MotoPlaca);
                if (moto == null)
                    return new ServiceResponse<Cliente> { Success = false, Message = "Moto n�o encontrada" };
            }

            var cliente = new Cliente
            {
                UsuarioCliente = clienteDto.UsuarioCliente,
                Nome = clienteDto.Nome,
                Senha = clienteDto.Senha,
                MotoPlaca = clienteDto.MotoPlaca
            };

            await _clienteRepository.AddAsync(cliente);
            return new ServiceResponse<Cliente>(cliente, "Cliente criado com sucesso!");
        }

        public async Task<ServiceResponse<Cliente>> UpdateClienteAsync(string usuarioCliente, ClienteDto clienteDto)
        {
            var clienteExistente = await _clienteRepository.GetByIdAsync(usuarioCliente);
            if (clienteExistente == null)
                return new ServiceResponse<Cliente> { Success = false, Message = "Cliente n�o encontrado" };

            if (!string.IsNullOrEmpty(clienteDto.MotoPlaca))
            {
                var moto = await _motoRepository.GetByIdAsync(clienteDto.MotoPlaca);
                if (moto == null)
                    return new ServiceResponse<Cliente> { Success = false, Message = "Moto n�o encontrada" };
            }

            clienteExistente.Nome = clienteDto.Nome;
            clienteExistente.Senha = clienteDto.Senha;
            clienteExistente.MotoPlaca = clienteDto.MotoPlaca;

            await _clienteRepository.UpdateAsync(clienteExistente);
            return new ServiceResponse<Cliente>(clienteExistente, "Cliente atualizado com sucesso!");
        }

        public async Task<ServiceResponse<bool>> DeleteClienteAsync(string usuarioCliente)
        {
            var cliente = await _clienteRepository.GetByIdAsync(usuarioCliente);
            if (cliente == null)
                return new ServiceResponse<bool> { Success = false, Message = "Cliente n�o encontrado" };

            await _clienteRepository.DeleteAsync(cliente);
            return new ServiceResponse<bool>(true, "Cliente exclu�do com sucesso!");
        }

        public async Task<ServiceResponse<IEnumerable<Cliente>>> GetClientesPaginatedAsync(int page, int pageSize)
        {
            var clientes = await _clienteRepository.GetPaginatedAsync(page, pageSize);
            return new ServiceResponse<IEnumerable<Cliente>>(clientes);
        }

        public async Task<ServiceResponse<int>> GetTotalClientesCountAsync()
        {
            var count = await _clienteRepository.GetTotalCountAsync();
            return new ServiceResponse<int>(count);
        }
    }
}