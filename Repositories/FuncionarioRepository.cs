using MottuApi.Data;
using MottuApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MottuApi.Repositories
{
    /// <summary>
    /// repositório para operações com funcionários.
    /// </summary>
    public class FuncionarioRepository : IFuncionarioRepository
    {
        private readonly MottuDbContext _context;

        public FuncionarioRepository(MottuDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Funcionario>> GetAllAsync()
        {
            return await _context.Funcionarios
                .Include(f => f.Patio)
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
    }
}