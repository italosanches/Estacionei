using AutoMapper;
using Estacionei.DTOs;
using Estacionei.DTOs.Customer;
using Estacionei.DTOs.Vehicle;
using Estacionei.DTOs.Veiculos;
using Estacionei.Enums;
using Estacionei.Extensions;
using Estacionei.Models;
using Estacionei.Pagination;
using Estacionei.Pagination.Parameters.Vehicle;
using Estacionei.Repository.Interfaces;
using Estacionei.Response;
using Estacionei.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net;
using System.Numerics;

namespace Estacionei.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public VehicleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ResponseBase<PagedList<VehicleResponseDto>>> GetAllAsync(VehicleQueryParameters queryParameters)
        {
            var vehicleQueryable = _unitOfWork.VehicleRepository.GetAllQueryable();

            if (queryParameters.CustomerId != 0)
            {
                vehicleQueryable = vehicleQueryable.Where(vehicle => vehicle.CustomerId == queryParameters.CustomerId);
            }
            if (!string.IsNullOrEmpty(queryParameters.CustomerName))
            {
                vehicleQueryable = vehicleQueryable.Where(vehicle => vehicle.Customer.CustomerName.ToUpper() == queryParameters.CustomerName.RemoveSpecialCharacters().ToUpper());
            }
            if (queryParameters.VehicleType != 0)
            {
                vehicleQueryable = vehicleQueryable.Where(vehicle => vehicle.VehicleType == queryParameters.VehicleType);
            }
            vehicleQueryable = vehicleQueryable.Include(customer => customer.Customer);
            var pagedVehicles = await PagedListService<VehicleResponseDto, Vehicle>.CreatePagedList(vehicleQueryable, queryParameters, _mapper);

            if (!pagedVehicles.Any())
            {
                return ResponseBase<PagedList<VehicleResponseDto>>.FailureResult("Não há veiculos cadastrados com os parametros passados.", HttpStatusCode.NotFound);
            }
            return ResponseBase<PagedList<VehicleResponseDto>>.SuccessResult(pagedVehicles, "Veiculos encontrados");

        }



        public async Task<ResponseBase<VehicleResponseDto>> GetVehicleByIdAsync(int id)
        {
            var vehicle = await _unitOfWork.VehicleRepository.GetAllQueryable().Where(vehicle => vehicle.VehicleId == id)
                                                                               .Include(customer => customer.Customer)
                                                                               .FirstOrDefaultAsync();
            if (vehicle == null)
            {
                return ResponseBase<VehicleResponseDto>.FailureResult("Veiculo não encontrado.", HttpStatusCode.NotFound);
            }
            return ResponseBase<VehicleResponseDto>.SuccessResult(_mapper.Map<VehicleResponseDto>(vehicle), "Veiculo encontrado");

        }

        public async Task<ResponseBase<VehicleResponseDto>> GetVehicleByLicensePlateAsync(string licensePlate)
        {
            var formattedlicensePlate = licensePlate.RemoveSpecialCharacters().ToUpper();
            var vehicle = await _unitOfWork.VehicleRepository.GetAllQueryable()
                                                            .Include(vehicle => vehicle.Customer)
                                                            .Where(v => v.VehicleLicensePlate == formattedlicensePlate)
                                                            .FirstOrDefaultAsync();

            if (vehicle == null)
            {
                return ResponseBase<VehicleResponseDto>.FailureResult("Veiculo não encontrado.", HttpStatusCode.NotFound);

            }
            return ResponseBase<VehicleResponseDto>.SuccessResult(_mapper.Map<VehicleResponseDto>(vehicle), "Veiculo encontrado");

        }


        public async Task<ResponseBase<VehicleResponseDto>> CreateVehicle(VehicleRequestDto vehicleCreateDto)
        {
            var vehicleExists = await CheckLicensePlate(vehicleCreateDto.VehicleLicensePlate);

            if (vehicleExists)
            {
                return ResponseBase<VehicleResponseDto>.FailureResult("Placa ja existe no banco de dados.", HttpStatusCode.BadRequest);
            }
            var customerSearched = await GetCustomer(vehicleCreateDto.CustomerId);
            if (customerSearched is null)
            {
                return ResponseBase<VehicleResponseDto>.FailureResult("Cliente não existe.", HttpStatusCode.NotFound);
            };

            var vehicle = _mapper.Map<Vehicle>(vehicleCreateDto);
            vehicle.VehicleLicensePlate = vehicle.VehicleLicensePlate.Replace(" ", "").ToUpper().RemoveSpecialCharacters();
            await _unitOfWork.VehicleRepository.AddAsync(vehicle);
            await _unitOfWork.Commit();
            await _unitOfWork.Dispose();
            var vehicleMapped = _mapper.Map<VehicleResponseDto>(vehicle);
            vehicleMapped.CustomerName = customerSearched.CustomerName;
            return ResponseBase<VehicleResponseDto>.SuccessResult(vehicleMapped, "Veiculo cadastrado com sucesso");

        }

        public async Task<ResponseBase<bool>> UpdateVehicleAsync(VehicleRequestDto vehicleUpdateDto)
        {
            var result = await _unitOfWork.VehicleRepository.GetAsync(x => x.VehicleId == vehicleUpdateDto.VehicleId);
            if (result == null)
            {
                return ResponseBase<bool>.FailureResult("Veiculo nao encontrado no banco de dados.", HttpStatusCode.BadRequest);
            }
            var vehicleExists = await CheckLicensePlate(vehicleUpdateDto.VehicleLicensePlate);
            if (vehicleExists)
            {
                return ResponseBase<bool>.FailureResult("Placa ja existe no banco de dados.", HttpStatusCode.BadRequest);
            }
            var clienteExist = await GetCustomer(vehicleUpdateDto.CustomerId);
            if (clienteExist is null)
            {
                return ResponseBase<bool>.FailureResult("Cliente não existe no banco de dados.", HttpStatusCode.BadRequest);

            }
            vehicleUpdateDto.VehicleLicensePlate = vehicleUpdateDto.VehicleLicensePlate.RemoveSpecialCharacters();
            _unitOfWork.VehicleRepository.UpdateAsync(_mapper.Map<Vehicle>(vehicleUpdateDto));
            await _unitOfWork.Commit();
            await _unitOfWork.Dispose();
            return ResponseBase<bool>.SuccessResult(true, "Veiculo atualizado com sucesso");

        }

        public async Task<ResponseBase<bool>> DeleteVehicleAsync(int id)
        {
            var vehicle = await _unitOfWork.VehicleRepository.GetAsync(vehicle => vehicle.VehicleId == id);
            if (vehicle != null)
            {
                _unitOfWork.VehicleRepository.DeleteAsync(vehicle);
                await _unitOfWork.Commit();
                await _unitOfWork.Dispose();
                return ResponseBase<bool>.SuccessResult(true, "Veiculo removido", HttpStatusCode.OK);
            }
            return ResponseBase<bool>.FailureResult("Veiculo nao existe no banco de dados", HttpStatusCode.BadRequest);

        }
        public async Task<bool> CheckLicensePlate(string licensePlate)
        {
            var vehicle = await _unitOfWork.VehicleRepository.GetAsync(vehicle => vehicle.VehicleLicensePlate == licensePlate.ToUpper().Replace(" ", ""));

            return vehicle != null;

        }
        public async Task<bool> CheckLicensePlate(Vehicle vehicle)
        {
            var checkVehicle = await _unitOfWork.VehicleRepository.GetAsync(v => v.VehicleLicensePlate == vehicle.VehicleLicensePlate.ToUpper().Replace(" ", ""));
            if (checkVehicle != null && (checkVehicle.VehicleId != vehicle.VehicleId))
            {
                return true;
            }

            return false;

        }
        private async Task<Customer> GetCustomer(int id)
        {
            return await _unitOfWork.CustomerRepository.GetAsync(x => x.CustomerId == id);

        }
        public bool ValidateVehicleType(int vehicleType)
        {
            return Enum.IsDefined(typeof(VehicleType), vehicleType);
        }
    }
}
