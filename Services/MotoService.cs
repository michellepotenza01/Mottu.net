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
    public class MotoService : IMotoService
    {
        private readonly IMotoRepository _motoRepository;
        private readonly IFuncionarioRepository _funcionarioRepository;
        private readonly IPatioRepository _patioRepository;

        public MotoService(IMotoRepository motoRepository, IFuncionarioRepository funcionarioRepository, IPatioRepository patioRepository)
        {
            _motoRepository = motoRepository;
            _funcionarioRepository = funcionarioRepository;
            _patioRepository = patioRepository;
        }

        /// <summary>
        /// Obtém todas as motos cadastradas.
        /// </summary>
        /// <returns>Lista de motos.</returns>
        [SwaggerResponse(200, "Motos encontradas", typeof(IEnumerable<Moto>))]
        public async Task<IEnumerable<Moto>> GetMotosAsync(string status = null, string setor = null)
        {
            var motos = _motoRepository.GetAllAsync();

            if (!string.IsNullOrEmpty(status))
                motos = motos.Where(m => m.Status == status);

            if (!string.IsNullOrEmpty(setor))
                motos = motos.Where(m => m.Setor == setor);

            return await motos.ToListAsync();
        }

        /// <summary>
        /// Obtém uma moto específica.
        /// </summary>
        /// <param name="placa">Placa da moto.</param>
        /// <returns>Moto encontrada.</returns>
        [SwaggerResponse(200, "Moto encontrada", typeof(Moto))]
        [SwaggerResponse(404, "Moto não encontrada")]
        public async Task<Moto> GetMotoAsync(string placa)
        {
            return await _motoRepository.GetByIdAsync(placa);
        }

        /// <summary>
        /// Cria uma nova moto.
        /// </summary>
        /// <param name="motoDto">Informações da moto.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        [SwaggerResponse(201, "Moto criada com sucesso")]
        [SwaggerResponse(400, "Erro ao criar moto")]
        public async Task<string> AddMotoAsync(MotoDto motoDto)
        {
            var funcionario = await _funcionarioRepository.GetByIdAsync(motoDto.UsuarioFuncionario);
            if (funcionario == null)
                return "Funcionário não encontrado.";

            var patio = await _patioRepository.GetByIdAsync(motoDto.NomePatio);
            if (patio == null)
                return "Pátio não encontrado.";

            if (patio.VagasOcupadas >= patio.VagasTotais)
                return "Não há vagas disponíveis no pátio.";

            var moto = new Moto
            {
                Placa = motoDto.Placa,
                Modelo = motoDto.Modelo,
                Status = motoDto.Status,
                Setor = motoDto.Setor,
                NomePatio = motoDto.NomePatio,
                UsuarioFuncionario = motoDto.UsuarioFuncionario,
                Funcionario = funcionario,
                Patio = patio
            };

            _motoRepository.AddAsync(moto);

            if (moto.Status == "Disponível" || moto.Status == "Manutenção")
                patio.VagasOcupadas++;

            await _patioRepository.SaveChangesAsync();
            return "Moto criada com sucesso!";
        }

        /// <summary>
        /// Atualiza uma moto existente.
        /// </summary>
        /// <param name="placa">Placa da moto.</param>
        /// <param name="motoDto">Novas informações da moto.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        [SwaggerResponse(200, "Moto atualizada com sucesso")]
        [SwaggerResponse(404, "Moto não encontrada")]
        public async Task<string> UpdateMotoAsync(string placa, MotoDto motoDto)
        {
            var motoExistente = await _motoRepository.GetByIdAsync(placa);
            if (motoExistente == null)
                return "Moto não encontrada.";

            var patio = motoExistente.Patio;

            if (motoDto.Status != motoExistente.Status)
            {
                if (motoDto.Status == "Alugada" && motoExistente.Status == "Disponível")
                    patio.VagasOcupadas--;
                else if (motoDto.Status == "Disponível" || motoDto.Status == "Manutenção" && motoExistente.Status == "Alugada")
                    patio.VagasOcupadas++;
            }

            motoExistente.Modelo = motoDto.Modelo;
            motoExistente.Status = motoDto.Status;
            motoExistente.Setor = motoDto.Setor;

            await _motoRepository.SaveChangesAsync();
            return "Moto atualizada com sucesso!";
        }

        /// <summary>
        /// Exclui uma moto do sistema.
        /// </summary>
        /// <param name="placa">Placa da moto.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        [SwaggerResponse(200, "Moto removida com sucesso")]
        [SwaggerResponse(404, "Moto não encontrada")]
        public async Task<string> DeleteMotoAsync(string placa)
        {
            var moto = await _motoRepository.GetByIdAsync(placa);
            if (moto == null)
                return "Moto não encontrada.";

            var patio = moto.Patio;
            if (patio != null && (moto.Status == "Disponível" || moto.Status == "Manutenção"))
                patio.VagasOcupadas--;

            await _motoRepository.DeleteAsync(moto);
            return "Moto removida com sucesso!";
        }
    }
}

