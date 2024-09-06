using Estacionei.DTOs;
using Estacionei.DTOs.Veiculos;
using Estacionei.Models;
using Estacionei.Response;

namespace Estacionei.Services.Interfaces
{
    public interface IVeiculoService
    {
        Task<ResponseBase<IEnumerable<VeiculoGetDto>>> GetAllVeiculoAsync();
        Task<ResponseBase<VeiculoGetDto>> GetVeiculoByIdAsync(int id);
        Task<ResponseBase<VeiculoGetDto>> GetVeiculoByPlacaAsync(string placa);

        Task<bool> CheckPlate(string placa);
        Task<bool> CheckPlate(Veiculo veiculo);
        Task<ResponseBase<VeiculoGetDto>> AddVeiculoAsync(VeiculoCreateDto veiculoCreateDto);
        Task<ResponseBase<VeiculoGetDto>> AddClienteVeiculoAsync(VeiculoCreateDto veiculoCreateDto);

        Task<ResponseBase<bool>> UpdateVeiculoAsync(VeiculoUpdateDto veiculoUpdateDto);
        Task<ResponseBase<bool>> DeleteVeiculoAsync(int id);
    }
}
