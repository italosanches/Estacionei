using Estacionei.Models;
using System.Linq.Expressions;

namespace Estacionei.Repository.Interfaces
{
	public interface IRepository<T> where T : class
	{
		Task<IEnumerable<T>> GetAllAsync();
		Task<T> GetByIdAsync(int id);
		Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
		Task<int> AddAsync(T entity);
		Task UpdateAsync(T entity);
		Task DeleteAsync(T entity);
	}
}
