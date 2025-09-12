using MottuApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MottuApi.Repositories
{
    public interface IClienteRepository
    {
        Task<IEnumerable<Cliente>> GetAllAsync();
        Task<IEnumerable<Cliente>> GetPaginatedAsync(int page, int pageSize);
        Task<Cliente> GetByIdAsync(string usuarioCliente);
        Task<Cliente> GetByIdWithMotoAsync(string usuarioCliente);
        Task AddAsync(Cliente cliente);
        Task UpdateAsync(Cliente cliente);
        Task DeleteAsync(Cliente cliente);
        Task<bool> ExistsAsync(string usuarioCliente);
        Task<int> GetTotalCountAsync();
    }
}