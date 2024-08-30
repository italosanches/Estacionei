using Estacionei.DTOs;
using Estacionei.DTOs.Veiculos;
using Estacionei.Models;
using Estacionei.Response;

namespace Estacionei.Services.Interfaces
{
    public interface IVeiculoService
    {
        Task<IEnumerable<Veiculo>> GetAllVeiculoAsync();
        Task<ResponseBase<VeiculoGetDto>> GetVeiculoByIdAsync(int id);
        //Task<Veiculo> GetVeiculoByPlacaAsync(string placa);
        Task<ResponseBase<Veiculo>> AddVeiculoAsync(VeiculoCreateDto veiculoCreateDto);
        Task<ResponseBase<bool>> UpdateVeiculoAsync(VeiculoUpdateDto veiculoUpdateDto);
        Task<ResponseBase<bool>> DeleteVeiculoAsync(int id);

    }
}
