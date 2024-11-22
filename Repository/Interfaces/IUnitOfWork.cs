namespace Estacionei.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        ICustomerRepository CustomerRepository { get; }
        IVehicleRepository VehicleRepository { get; }

        IEntryRepository EntryRepository { get; }
        IExitRepository ExitRepository { get; }

        IHourPriceConfigurationRepository HourPriceConfigurationRepository { get; }


        Task Commit();
        Task Dispose();
    }
}
