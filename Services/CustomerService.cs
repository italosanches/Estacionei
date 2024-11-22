using AutoMapper;
using Estacionei.DTOs;
using Estacionei.DTOs.Cliente;
using Estacionei.DTOs.Customer;
using Estacionei.Extensions;
using Estacionei.Mapping;
using Estacionei.Models;
using Estacionei.Pagination;
using Estacionei.Pagination.Parameters.ClienteParameters;

using Estacionei.Repository.Interfaces;
using Estacionei.Response;
using Estacionei.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;

namespace Estacionei.Services
{
    public class CustomerService : ICustomerService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomerService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseBase<PagedList<CustomerResponseDto>>> GetAllCustomersByPaginationAsync(CustomerQueryParameters customerQueryParameters)
        {
            var customersQueryable = _unitOfWork.CustomerRepository.GetAllQueryable().AsNoTracking();

            if (customerQueryParameters.CustomerName is not null)
            {
                customersQueryable = customersQueryable.Where(customer => customer.CustomerName.ToUpper().Contains(customerQueryParameters.CustomerName.ToUpper().RemoveSpecialCharacters()));
            }

            customersQueryable = customersQueryable.Include(cli => cli.CustomerVehicles);
            // Obtém a lista paginada
            var pagedCustomers = await PagedListService<CustomerResponseDto, Customer>.CreatePagedList(customersQueryable, customerQueryParameters, _mapper);

            if (!pagedCustomers.Any())
            {
                return ResponseBase<PagedList<CustomerResponseDto>>.FailureResult("Não há registros no banco.", HttpStatusCode.NotFound);
            }

            return ResponseBase<PagedList<CustomerResponseDto>>.SuccessResult(pagedCustomers, "Clientes paginados");
        }


        public async Task<ResponseBase<CustomerResponseDto>> GetCustomerByIdAsync(int id)
        {
            var customer = await _unitOfWork.CustomerRepository.GetCustomerAndVehicles(id);
            if (customer == null)
            {
                return ResponseBase<CustomerResponseDto>.FailureResult("Cliente não encontrado", HttpStatusCode.NotFound);
            }

            return ResponseBase<CustomerResponseDto>.SuccessResult(_mapper.Map<CustomerResponseDto>(customer), "Cliente encontrado");
        }

        public async Task<ResponseBase<CustomerResponseDto>> AddCustomerAsync(CustomerRequestCreateDto customerDto)
        {
            var customer = _mapper.Map<Customer>(customerDto);

            if (customerDto.Vehicle == null)
            {
                await _unitOfWork.CustomerRepository.AddAsync(customer);
                await _unitOfWork.Commit();
                await _unitOfWork.Dispose();


                return ResponseBase<CustomerResponseDto>.SuccessResult(_mapper.Map<CustomerResponseDto>(customer), "Cliente cadastrado com sucesso.");
            }
            else
            {
                var vehicleMapped = _mapper.Map<Vehicle>(customerDto.Vehicle);
                var veiculoExist = await _unitOfWork.VehicleRepository.GetVehicleByLicensePlateAsync(vehicleMapped.VehicleLicensePlate.Replace(" ", "").ToUpper().RemoveSpecialCharacters());
                if (veiculoExist != null)
                {
                    return ResponseBase<CustomerResponseDto>.FailureResult("Placa ja existe no banco de dados.", HttpStatusCode.BadRequest);
                }
                await _unitOfWork.CustomerRepository.AddAsync(customer);
                await _unitOfWork.Commit();
                vehicleMapped.CustomerId = customer.CustomerId;
                vehicleMapped.VehicleLicensePlate = vehicleMapped.VehicleLicensePlate.RemoveSpecialCharacters().Replace(" ", "").ToUpper();

                await _unitOfWork.VehicleRepository.AddAsync(vehicleMapped);
                await _unitOfWork.Commit();
                await _unitOfWork.Dispose();
                return ResponseBase<CustomerResponseDto>.SuccessResult(_mapper.Map<CustomerResponseDto>(customer), "Cliente cadastrado com sucesso.");

            }
        }

        public async Task<ResponseBase<bool>> UpdateCustomerAsync(CustomerRequestUpdateDto customerDto)
        {
            var customer = await _unitOfWork.CustomerRepository.GetCustomerAndVehicles(customerDto.CustomerId);
            if (customer == null)
            {
                return ResponseBase<bool>.FailureResult("Cliente não encontrado.", HttpStatusCode.NotFound);
            }
            _unitOfWork.CustomerRepository.UpdateAsync(_mapper.Map<Customer>(customerDto));
            await _unitOfWork.Commit();
            await _unitOfWork.Dispose();
            return ResponseBase<bool>.SuccessResult(true, "Cliente atualizado com sucesso.");
        }
        public async Task<ResponseBase<bool>> DeleteCustomerAsync(int id)
        {
            var customer = await _unitOfWork.CustomerRepository.GetCustomerAndVehicles(id);
            if (customer == null)
            {
                return ResponseBase<bool>.FailureResult("Cliente não encontrado.", HttpStatusCode.NotFound);
            }
            _unitOfWork.CustomerRepository.DeleteAsync(customer);
            await _unitOfWork.Commit();
            await _unitOfWork.Dispose();
            return ResponseBase<bool>.SuccessResult(true, "Cliente deletado com sucesso.");


        }
    }
}

