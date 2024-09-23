using Estacionei.Models;
using System.Linq.Expressions;

namespace Estacionei.Repository.Interfaces
{
	public interface IRepository<T>
	{
		Task<IEnumerable<T>> GetAllAsync();
		IQueryable<T> GetAllQueryable();
		Task<T?> GetAsync(Expression<Func<T,bool>> predicate);
		Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
		Task<T> AddAsync(T entity);
		T UpdateAsync(T entity);
		T DeleteAsync(T entity);
	}
}
