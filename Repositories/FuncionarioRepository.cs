using MottuApi.Data;
using MottuApi.Models;
using Microsoft.EntityFrameworkCore;

namespace MottuApi.Repositories
{
    public class FuncionarioRepository
    {
        private readonly MottuDbContext _context;

        public FuncionarioRepository(MottuDbContext context)
        {
            _context = context;
        }

        // ✅ TODOS OS MÉTODOS NORMAIS COM LINQ
        public async Task<List<Funcionario>> GetAllAsync()
        {
            return await _context.Funcionarios
                .Include(f => f.Patio)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Funcionario?> GetByIdAsync(string usuarioFuncionario)
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
            // ✅ ÚNICO SQL DIRETO - SÓ PARA EXISTS
            try
            {
                var sql = "SELECT COUNT(*) FROM \"Funcionarios\" WHERE \"UsuarioFuncionario\" = :p0";
                var count = await _context.Database.SqlQueryRaw<int>(sql, usuarioFuncionario).FirstOrDefaultAsync();
                return count > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Funcionario?> GetByUsernameAsync(string usuario)
        {
            return await _context.Funcionarios
                .Include(f => f.Patio)
                .FirstOrDefaultAsync(f => f.UsuarioFuncionario == usuario);
        }

        public async Task<List<Funcionario>> GetByPatioAsync(string nomePatio)
        {
            return await _context.Funcionarios
                .Where(f => f.NomePatio == nomePatio)
                .Include(f => f.Patio)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> PertenceAoPatioAsync(string usuarioFuncionario, string nomePatio)
        {
            // ✅ AQUI PODE SER LINQ NORMAL (não gera TRUE/FALSE)
            return await _context.Funcionarios
                .AnyAsync(f => f.UsuarioFuncionario == usuarioFuncionario && f.NomePatio == nomePatio);
        }
    }
}