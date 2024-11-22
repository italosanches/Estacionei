using Estacionei.DTOs;
using Estacionei.DTOs.Vehicle;
using Estacionei.DTOs.Veiculos;
using Estacionei.Enums;
using Estacionei.Models;
using Estacionei.Pagination;
using Estacionei.Pagination.Parameters.Vehicle;
using Estacionei.Response;

namespace Estacionei.Services.Interfaces
{
    public interface IVehicleService
    {
        Task<ResponseBase<PagedList<VehicleResponseDto>>> GetAllAsync(VehicleQueryParameters queryParameters);
        

        Task<ResponseBase<VehicleResponseDto>> GetVehicleByIdAsync(int id);
        Task<ResponseBase<VehicleResponseDto>> GetVehicleByLicensePlateAsync(string licensePlate);
        

        Task<bool> CheckLicensePlate(string licensePlate);
        Task<bool> CheckLicensePlate(Vehicle vehicle);
        Task<ResponseBase<VehicleResponseDto>> CreateVehicle(VehicleRequestDto vehicleCreateDto);
        //Task<ResponseBase<VeiculoGetDto>> AddClienteVeiculoAsync(VeiculoRequestDto veiculoCreateDto);

        Task<ResponseBase<bool>> UpdateVehicleAsync(VehicleRequestDto vehicleUpdateDto);
        Task<ResponseBase<bool>> DeleteVehicleAsync(int id);

        public bool ValidateVehicleType(int vehicleType);
    }
}
