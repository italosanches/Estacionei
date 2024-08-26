using Estacionei.Models;

namespace Estacionei.Repository.Interfaces
{
    public interface IVeiculoRepository
    {
        Task<IEnumerable<Veiculo>> GetAllAsync();
        Task<Veiculo> GetByIdAsync(int id);

        Task<Veiculo> GetByPlacaAsync(string placa);
        Task<int> AddAsync(Veiculo veiculo);
        Task UpdateAsync(Veiculo veiculo);
        Task DeleteAsync(string placa);
        

    }
}
