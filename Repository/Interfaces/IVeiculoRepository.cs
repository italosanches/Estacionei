using Estacionei.Models;

namespace Estacionei.Repository.Interfaces
{
    public interface IVeiculoRepository : IRepository<Veiculo>
    {
        Task<Veiculo?> GetVeiculoByPlaca(string placa);
    }
}
