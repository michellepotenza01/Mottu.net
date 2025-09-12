using MottuApi.Data;
using MottuApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MottuApi.Repositories
{
    /// <summary>
    /// Implementação do repositório para operações com motos.
    /// </summary>
    public class MotoRepository : IMotoRepository
    {
        private readonly MottuDbContext _context;

        public MotoRepository(MottuDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Moto>> GetAllAsync()
        {
            return await _context.Motos
                .Include(m => m.Patio)
                .Include(m => m.Funcionario)
                .ToListAsync();
        }

        public async Task<Moto> GetByIdAsync(string placa)
        {
            return await _context.Motos
                .Include(m => m.Patio)
                .Include(m => m.Funcionario)
                .FirstOrDefaultAsync(m => m.Placa == placa);
        }

        public async Task AddAsync(Moto moto)
        {
            await _context.Motos.AddAsync(moto);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Moto moto)
        {
            _context.Motos.Update(moto);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Moto moto)
        {
            _context.Motos.Remove(moto);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(string placa)
        {
            return await _context.Motos.AnyAsync(m => m.Placa == placa);
        }

        public async Task<IEnumerable<Moto>> GetByPatioAsync(string nomePatio)
        {
            return await _context.Motos
                .Where(m => m.NomePatio == nomePatio)
                .ToListAsync();
        }
    }
}
