using Estacionei.Models;

namespace Estacionei.Repository.Interfaces
{
    public interface IVeiculoRepository : IRepository<Veiculo>
    {
        Task<Veiculo?> GetVeiculoByPlacaAsync(string placa);
        Task<IEnumerable<Veiculo?>> GetVeiculoByClienteAsync(int id);
    }
}
