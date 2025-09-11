using MottuApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MottuApi.Repositories
{
    /// <summary>
    /// Interface do reposit�rio para opera��es com funcion�rios.
    /// </summary>
    public interface IFuncionarioRepository
    {
        Task<IEnumerable<Funcionario>> GetAllAsync();
        Task<Funcionario> GetByIdAsync(string usuarioFuncionario);
        Task AddAsync(Funcionario funcionario);
        Task UpdateAsync(Funcionario funcionario);
        Task DeleteAsync(Funcionario funcionario);
    }
}
