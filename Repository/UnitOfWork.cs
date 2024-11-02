using Estacionei.Context;
using Estacionei.Repository.Interfaces;

namespace Estacionei.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private IClienteRepository _clienteRepo;
        private IVeiculoRepository _veiculoRepo;
        private IConfiguracaoValorHoraRepository _configuracaoValorHoraRepo;
        private IEntradaRepository _entradaRepo;
        private ISaidaRepository _saidaRepo;

        public AppDbContext _context;

        
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }
        public IClienteRepository ClienteRepository
        {
            get { return _clienteRepo = _clienteRepo ??  new ClienteRepository(_context); }
        }

        public IVeiculoRepository VeiculoRepository
        {
            get { return _veiculoRepo = _veiculoRepo ??  new VeiculoRepository(_context); }
        }

        public IConfiguracaoValorHoraRepository ConfiguracaoValorHoraRepository
        {
            get { return _configuracaoValorHoraRepo = _configuracaoValorHoraRepo ?? new ConfiguracaoValorHoraRepository(_context); }
        }

        public IEntradaRepository EntradaRepository 
        {
            get { return _entradaRepo  = _entradaRepo ?? new EntradaRepository(_context); }
        }

        public ISaidaRepository SaidaRepository
        {
            get { return _saidaRepo = _saidaRepo ?? new SaidaRepository(_context); }
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
