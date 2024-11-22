using Estacionei.Models;

namespace Estacionei.Repository.Interfaces
{
    public interface IVehicleRepository : IRepository<Vehicle>
    {
        Task<Vehicle?> GetVehicleByLicensePlateAsync(string licensePlate);
       
    }
}
