using Estacionei.Context;
using Estacionei.Models;
using Estacionei.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Estacionei.Repository
{
    public class VeiculoRepository : IVeiculoRepository
    {
        private readonly AppDbContext _context;

        public VeiculoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(Veiculo veiculo)
        {
            try
            {
                await _context.Veiculos.AddAsync(veiculo);
                _context.SaveChanges();
            }
            catch (Exception ex) 
            { 
                new Exception("Erro ao gravar veiculo" + ex.Message, ex);
            }
            return veiculo.VeiculoId;

        }

        public Task DeleteAsync(string placa)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Veiculo>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Veiculo> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Veiculo> GetByPlacaAsync(string placa)
        {
          return await _context.Veiculos.AsNoTracking().FirstOrDefaultAsync(x=> x.VeiculoPlaca == placa);
        }

        public Task UpdateAsync(Veiculo veiculo)
        {
            throw new NotImplementedException();
        }
    }
}
