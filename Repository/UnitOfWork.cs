using Estacionei.Context;
using Estacionei.Repository.Interfaces;

namespace Estacionei.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private IClienteRepository clienteRepo;
        private IVeiculoRepository veiculoRepo;
        private IConfiguracaoValorHoraRepository configuracaoValorHoraRepo;

        public AppDbContext _context;

        
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }
        public IClienteRepository ClienteRepository
        {
            get { return clienteRepo = clienteRepo ??  new ClienteRepository(_context); }
        }

        public IVeiculoRepository VeiculoRepository
        {
            get { return veiculoRepo = veiculoRepo ??  new VeiculoRepository(_context); }
        }

        public IConfiguracaoValorHoraRepository configuracaoValorHoraRepository
        {
            get { return configuracaoValorHoraRepo = configuracaoValorHoraRepo ?? new ConfiguracaoValorHoraRepository(_context); }
        }

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Dispose()
        {
           await _context.DisposeAsync();
        }
    }
}
