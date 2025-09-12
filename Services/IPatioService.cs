using MottuApi.Models;
using MottuApi.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MottuApi.Services
{
    public interface IPatioService
    {
        Task<ServiceResponse<IEnumerable<Patio>>> GetPatiosAsync();
        Task<ServiceResponse<Patio>> GetPatioAsync(string nomePatio);
        Task<ServiceResponse<Patio>> CreatePatioAsync(PatioDto patioDto);
        Task<ServiceResponse<Patio>> UpdatePatioAsync(string nomePatio, PatioDto patioDto);
        Task<ServiceResponse<bool>> DeletePatioAsync(string nomePatio);


        Task<ServiceResponse<IEnumerable<Patio>>> GetPatiosPaginatedAsync(int page, int pageSize);
        Task<ServiceResponse<int>> GetTotalPatiosCountAsync();


        Task<ServiceResponse<bool>> VerificarVagasDisponiveisAsync(string nomePatio);
        Task<ServiceResponse<int>> GetVagasDisponiveisAsync(string nomePatio);
    }
}