using MottuApi.Models;
using MottuApi.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MottuApi.Services
{
    public interface IFuncionarioService
    {
        Task<ServiceResponse<IEnumerable<Funcionario>>> GetFuncionariosAsync();
        Task<ServiceResponse<Funcionario>> GetFuncionarioByIdAsync(string usuarioFuncionario);
        Task<ServiceResponse<Funcionario>> CreateFuncionarioAsync(FuncionarioDto funcionarioDto);
        Task<ServiceResponse<Funcionario>> UpdateFuncionarioAsync(string usuarioFuncionario, FuncionarioDto funcionarioDto);
        Task<ServiceResponse<bool>> DeleteFuncionarioAsync(string usuarioFuncionario);

        Task<ServiceResponse<IEnumerable<Funcionario>>> GetFuncionariosPaginatedAsync(int page, int pageSize);
        Task<ServiceResponse<int>> GetTotalFuncionariosCountAsync();
    }
}