using MottuApi.Models;
using MottuApi.DTOs;
using MottuApi.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MottuApi.Services
{
    public interface IMotoService
    {
        Task<ServiceResponse<IEnumerable<Moto>>> GetMotosAsync(StatusMoto? status = null, SetorMoto? setor = null);
        Task<ServiceResponse<Moto>> GetMotoAsync(string placa);
        Task<ServiceResponse<Moto>> CreateMotoAsync(MotoDto motoDto);
        Task<ServiceResponse<Moto>> UpdateMotoAsync(string placa, MotoDto motoDto);
        Task<ServiceResponse<bool>> DeleteMotoAsync(string placa);

        Task<ServiceResponse<IEnumerable<Moto>>> GetMotosPaginatedAsync(int page, int pageSize, StatusMoto? status = null, SetorMoto? setor = null);
        Task<ServiceResponse<int>> GetTotalMotosCountAsync(StatusMoto? status = null, SetorMoto? setor = null);
        Task<ServiceResponse<IEnumerable<Moto>>> GetMotosByPatioAsync(string nomePatio);
    }
}