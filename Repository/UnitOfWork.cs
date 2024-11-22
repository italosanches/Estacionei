using Estacionei.Context;
using Estacionei.Repository.Interfaces;

namespace Estacionei.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ICustomerRepository _customerRepo;
        private IVehicleRepository _vehicleRepo;
        private IHourPriceConfigurationRepository _hourPriceConfigurationRepo;
        private IEntryRepository _entryRepo;
        private IExitRepository _exitRepo;

        public AppDbContext _context;

        
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }
        public ICustomerRepository CustomerRepository
        {
            get { return _customerRepo = _customerRepo ??  new CustomerRepository(_context); }
        }

        public IVehicleRepository VehicleRepository
        {
            get { return _vehicleRepo = _vehicleRepo ??  new VehicleRepository(_context); }
        }

        public IHourPriceConfigurationRepository HourPriceConfigurationRepository
        {
            get { return _hourPriceConfigurationRepo = _hourPriceConfigurationRepo ?? new HourPriceConfigurationRepository(_context); }
        }

        public IEntryRepository EntryRepository 
        {
            get { return _entryRepo  = _entryRepo ?? new EntryRepository(_context); }
        }

        public IExitRepository ExitRepository
        {
            get { return _exitRepo = _exitRepo ?? new ExitRepository(_context); }
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
