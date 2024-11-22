
using Estacionei.Models;

namespace Estacionei.Repository.Interfaces
{
    public interface ICustomerRepository : IRepository<Customer>
    {
       Task<IEnumerable<Customer>> GetAllCustomersAndVehicles();
       Task<Customer?> GetCustomerAndVehicles(int id);
    }
}
