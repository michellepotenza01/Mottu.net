using MottuApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MottuApi.Repositories
{
    /// <summary>
    /// Interface do reposit�rio para opera��es com p�tios.
    /// </summary>
    public interface IPatioRepository
    {
        Task<IEnumerable<Patio>> GetAllAsync();
        Task<Patio> GetByIdAsync(string nomePatio);
        Task AddAsync(Patio patio);
        Task UpdateAsync(Patio patio);
        Task DeleteAsync(Patio patio);
        Task<bool> ExistsAsync(string nomePatio);
    }
}