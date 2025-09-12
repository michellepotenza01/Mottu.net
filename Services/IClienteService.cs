using MottuApi.Models;
using MottuApi.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MottuApi.Services
{
    public interface IClienteService
    {
        Task<ServiceResponse<IEnumerable<Cliente>>> GetClientesAsync();
        Task<ServiceResponse<Cliente>> GetClienteByIdAsync(string usuarioCliente);
        Task<ServiceResponse<Cliente>> CreateClienteAsync(ClienteDto clienteDto);
        Task<ServiceResponse<Cliente>> UpdateClienteAsync(string usuarioCliente, ClienteDto clienteDto);
        Task<ServiceResponse<bool>> DeleteClienteAsync(string usuarioCliente);

        Task<ServiceResponse<IEnumerable<Cliente>>> GetClientesPaginatedAsync(int page, int pageSize);
        Task<ServiceResponse<int>> GetTotalClientesCountAsync();
    }
}