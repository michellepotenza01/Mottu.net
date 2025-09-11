using MottuApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MottuApi.Repositories
{
    /// <summary>
    /// Interface do repositório para operações com motos.
    /// </summary>
    public interface IMotoRepository
    {
        Task<IEnumerable<Moto>> GetAllAsync();
        Task<Moto> GetByIdAsync(string placa);
        Task AddAsync(Moto moto);
        Task UpdateAsync(Moto moto);
        Task DeleteAsync(Moto moto);
        Task<IEnumerable<Moto>> GetByPatioAsync(string nomePatio);
    }
}
