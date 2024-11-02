using Estacionei.DTOs;
using Estacionei.DTOs.Veiculo;
using Estacionei.DTOs.Veiculos;
using Estacionei.Models;
using Estacionei.Pagination;
using Estacionei.Pagination.Parameters.Veiculo;
using Estacionei.Response;

namespace Estacionei.Services.Interfaces
{
    public interface IVeiculoService
    {
        Task<ResponseBase<PagedList<VeiculoResponseDto>>> GetAllAsync(VeiculoQueryParameters queryParameters);
        

        Task<ResponseBase<VeiculoResponseDto>> GetVeiculoByIdAsync(int id);
        Task<ResponseBase<VeiculoResponseDto>> GetVeiculoByPlacaAsync(string placa);
        

        Task<bool> CheckPlate(string placa);
        Task<bool> CheckPlate(Veiculo veiculo);
        Task<ResponseBase<VeiculoResponseDto>> CreateVehicle(VeiculoRequestDto veiculoCreateDto);
        //Task<ResponseBase<VeiculoGetDto>> AddClienteVeiculoAsync(VeiculoRequestDto veiculoCreateDto);

        Task<ResponseBase<bool>> UpdateVeiculoAsync(VeiculoRequestDto veiculoUpdateDto);
        Task<ResponseBase<bool>> DeleteVeiculoAsync(int id);

        public bool ValidateTypeVehicle(int tipoVeiculo);
    }
}
