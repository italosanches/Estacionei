using Estacionei.Context;
using Estacionei.Models;
using Estacionei.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Estacionei.Repository
{
	public class CustomerRepository : Repository<Customer> , ICustomerRepository
	{

        public CustomerRepository(AppDbContext context) : base(context)
		{
			
		}

        public async Task<IEnumerable<Customer>> GetAllCustomersAndVehicles()
        {
            return await _context.Customers.AsNoTracking().Include(x => x.CustomerVehicles).ToListAsync();
        }
        public async Task<Customer?> GetCustomerAndVehicles(int id)
        {
            return await _context.Customers.AsNoTracking().Include(x => x.CustomerVehicles).FirstOrDefaultAsync(x => x.CustomerId == id);
        }
    }
}
