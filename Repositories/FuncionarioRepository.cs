using MottuApi.Data;
using MottuApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MottuApi.Repositories
{
    public class PatioRepository
    {
        private readonly MottuDbContext _context;

        public PatioRepository(MottuDbContext context)
        {
            _context = context;
        }

        public async Task<List<Patio>> GetAllAsync()
        {
            return await _context.Patios.ToListAsync();
        }

        public async Task<List<Patio>> GetPaginatedAsync(int page, int pageSize)
        {
            return await _context.Patios
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
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

        public async Task<bool> ExistsAsync(string nomePatio)
        {
            return await _context.Patios.AnyAsync(p => p.NomePatio == nomePatio);
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Patios.CountAsync();
        }
    }
}