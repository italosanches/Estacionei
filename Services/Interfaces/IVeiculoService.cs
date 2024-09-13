using Estacionei.DTOs;
using Estacionei.DTOs.Veiculo;
using Estacionei.DTOs.Veiculos;
using Estacionei.Models;
using Estacionei.Response;

namespace Estacionei.Services.Interfaces
{
    public interface IVeiculoService
    {
        Task<ResponseBase<IEnumerable<VeiculoResponseDto>>> GetAllVeiculoAsync();
        Task<ResponseBase<IEnumerable<VeiculoResponseDto>>> GetAllVeiculoByClienteAsync(int clienteId);

        Task<ResponseBase<VeiculoResponseDto>> GetVeiculoByIdAsync(int id);
        Task<ResponseBase<VeiculoResponseDto>> GetVeiculoByPlacaAsync(string placa);
        

        Task<bool> CheckPlate(string placa);
        Task<bool> CheckPlate(Veiculo veiculo);
        Task<ResponseBase<VeiculoResponseDto>> AddVeiculoAsync(VeiculoRequestCreateDto veiculoCreateDto);
        //Task<ResponseBase<VeiculoGetDto>> AddClienteVeiculoAsync(VeiculoRequestCreateDto veiculoCreateDto);

        Task<ResponseBase<bool>> UpdateVeiculoAsync(VeiculoRequestUpdateDto veiculoUpdateDto);
        Task<ResponseBase<bool>> DeleteVeiculoAsync(int id);
    }
}
