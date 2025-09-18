using MottuApi.Data;
using MottuApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MottuApi.Repositories
{
    public class FuncionarioRepository
    {
        private readonly MottuDbContext _context;

        public FuncionarioRepository(MottuDbContext context)
        {
            _context = context;
        }

        public async Task<List<Funcionario>> GetAllAsync()
        {
            return await _context.Funcionarios
                .Include(f => f.Patio)
                .ToListAsync();
        }

        public async Task<List<Funcionario>> GetPaginatedAsync(int page, int pageSize)
        {
            return await _context.Funcionarios
                .Include(f => f.Patio)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Funcionario> GetByIdAsync(string usuarioFuncionario)
        {
            return await _context.Funcionarios
                .Include(f => f.Patio)
                .FirstOrDefaultAsync(f => f.UsuarioFuncionario == usuarioFuncionario);
        }

        public async Task AddAsync(Funcionario funcionario)
        {
            await _context.Funcionarios.AddAsync(funcionario);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Funcionario funcionario)
        {
            _context.Funcionarios.Update(funcionario);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Funcionario funcionario)
        {
            _context.Funcionarios.Remove(funcionario);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(string usuarioFuncionario)
        {
            return await _context.Funcionarios.AnyAsync(f => f.UsuarioFuncionario == usuarioFuncionario);
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Funcionarios.CountAsync();
        }

        public async Task<List<Funcionario>> GetByPatioAsync(string nomePatio)
        {
            return await _context.Funcionarios
                .Where(f => f.NomePatio == nomePatio)
                .Include(f => f.Patio)
                .ToListAsync();
        }
    }
}