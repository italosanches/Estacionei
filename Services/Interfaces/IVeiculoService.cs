using Estacionei.DTOs;
using Estacionei.Models;
using Estacionei.Response;

namespace Estacionei.Services.Interfaces
{
    public interface IVeiculoService
    {
        Task<IEnumerable<Veiculo>> GetAllVeiculoAsync();
        Task<Veiculo> GetVeiculoByIdAsync(int id);
        //Task<Veiculo> GetVeiculoByPlacaAsync(string placa);
        Task<ResponseBase<Veiculo>> AddVeiculoAsync(VeiculoCreateDto veiculoCreateDto);
        Task UpdateVeiculoAsync(Veiculo veiculo);
        Task DeleteVeiculoAsync(int id);

    }
}
