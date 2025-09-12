using MottuApi.Data;
using MottuApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MottuApi.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly MottuDbContext _context;

        public ClienteRepository(MottuDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cliente>> GetAllAsync()
        {
            return await _context.Clientes
                .Include(c => c.Moto)
                .ToListAsync();
        }

        public async Task<IEnumerable<Cliente>> GetPaginatedAsync(int page, int pageSize)
        {
            return await _context.Clientes
                .Include(c => c.Moto)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Cliente> GetByIdAsync(string usuarioCliente)
        {
            return await _context.Clientes
                .Include(c => c.Moto)
                .FirstOrDefaultAsync(c => c.UsuarioCliente == usuarioCliente);
        }

        public async Task<Cliente> GetByIdWithMotoAsync(string usuarioCliente)
        {
            return await _context.Clientes
                .Include(c => c.Moto)
                .FirstOrDefaultAsync(c => c.UsuarioCliente == usuarioCliente);
        }

        public async Task AddAsync(Cliente cliente)
        {
            await _context.Clientes.AddAsync(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Cliente cliente)
        {
            _context.Clientes.Update(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Cliente cliente)
        {
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(string usuarioCliente)
        {
            return await _context.Clientes.AnyAsync(c => c.UsuarioCliente == usuarioCliente);
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Clientes.CountAsync();
        }
    }
}