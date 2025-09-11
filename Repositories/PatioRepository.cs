using MottuApi.Data;
using MottuApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MottuApi.Repositories
{
    /// <summary>
    /// Implementação do repositório para operações com pátios.
    /// </summary>
    public class PatioRepository : IPatioRepository
    {
        private readonly MottuDbContext _context;

        public PatioRepository(MottuDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Patio>> GetAllAsync()
        {
            return await _context.Patios.ToListAsync();
        }

        public async Task<Patio> GetByIdAsync(string nomePatio)
        {
            return await _context.Patios
                .FirstOrDefaultAsync(p => p.NomePatio == nomePatio);
        }

        public async Task AddAsync(Patio patio)
        {
            await _context.Patios.AddAsync(patio);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Patio patio)
        {
            _context.Patios.Update(patio);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Patio patio)
        {
            _context.Patios.Remove(patio);
            await _context.SaveChangesAsync();
        }
    }
}
