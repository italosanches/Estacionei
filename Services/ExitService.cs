using AutoMapper;
using Azure;
using Estacionei.DTOs.Cliente;
using Estacionei.DTOs.Saida;
using Estacionei.Enums;
using Estacionei.Extensions;
using Estacionei.Models;
using Estacionei.Pagination;
using Estacionei.Pagination.Parameters;
using Estacionei.Pagination.Parameters.SaidaParameters;
using Estacionei.Repository;
using Estacionei.Repository.Interfaces;
using Estacionei.Response;
using Estacionei.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Estacionei.Services
{
    public class ExitService : IExitService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public ExitService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseBase<ExitResponseDto>> CreateExit(ExitRequestDto exitRequestDto)
        {
            var entrySearched = await _unitOfWork.EntryRepository.GetAllQueryable().Where(entry => entry.EntryId == exitRequestDto.EntryId && entry.EntryStatus != EntryStatus.Closed)
                                                                               .Include(vehicle => vehicle.Vehicle)
                                                                               .ThenInclude(customer => customer.Customer)
                                                                               .FirstOrDefaultAsync();
            if (entrySearched == null)
            {
                return ResponseBase<ExitResponseDto>.FailureResult("Entrada não encontrada, ou entrada esta finalizada.", HttpStatusCode.BadRequest);
            }

            if (exitRequestDto.ExitDate < entrySearched.EntryDate)
            {
                return ResponseBase<ExitResponseDto>.FailureResult($"Data de saida {exitRequestDto.ExitDate} esta maior que a data de entrada {entrySearched.EntryDate}", HttpStatusCode.BadRequest);
            }
            var exit = _mapper.Map<Exit>(exitRequestDto);
            exit.ChargedAmount = await _unitOfWork.ExitRepository.SearchVehicleAndCalculateTheAmountPay(entrySearched.VehicleId, entrySearched.EntryDate, exitRequestDto.ExitDate);
            entrySearched.EntryStatus = EntryStatus.Closed;

            await _unitOfWork.ExitRepository.AddAsync(exit);
            _unitOfWork.EntryRepository.UpdateAsync(entrySearched);

            await _unitOfWork.Commit();
            await _unitOfWork.Dispose();

            var exitMapped = _mapper.Map<ExitResponseDto>(exit);

            return ResponseBase<ExitResponseDto>.SuccessResult(exitMapped, "Saida criada");


        }

        public async Task<ResponseBase<bool>> DeleteExit(int exitId)
        {
            var exitSearched = await _unitOfWork.ExitRepository.GetAsync(exit => exit.ExitId == exitId);
            if (exitSearched == null)
            {
                return ResponseBase<bool>.FailureResult("Saida não encontrada.", HttpStatusCode.NotFound);
            }
            var entrySearched = await _unitOfWork.EntryRepository.GetAsync(entry => entry.EntryId == exitSearched.EntryId);
            entrySearched.EntryStatus = EntryStatus.Open;
            _unitOfWork.EntryRepository.UpdateAsync(entrySearched);
            _unitOfWork.ExitRepository.DeleteAsync(exitSearched);
            await _unitOfWork.Commit();
            await _unitOfWork.Dispose();
            return ResponseBase<bool>.SuccessResult(true, $"Saida removida e alterado a entrada {entrySearched.EntryId} para em aberto");
        }

        public async Task<ResponseBase<PagedList<ExitResponseDto>>> GetAllExits(ExitQueryParameters exitQueryParameters)
        {
            var exitQuery = _unitOfWork.ExitRepository.GetAllQueryable();

            if (exitQueryParameters.EndDate != DateTime.MinValue && exitQueryParameters.StartDate > exitQueryParameters.EndDate)
            {
                return ResponseBase<PagedList<ExitResponseDto>>.FailureResult("Data de início não pode ser maior que a data de fim", HttpStatusCode.BadRequest);
            }
            else
            {   //Filtranda pela data inicio e fim, caso nao seja passado nenhum valor = min value, entao atribui minvalue
                exitQuery = exitQuery.Where(exit => (exitQueryParameters.StartDate == DateTime.MinValue || exit.ExitDate >= exitQueryParameters.StartDate) &&
                                                       (exitQueryParameters.EndDate == DateTime.MinValue || exit.ExitDate <= exitQueryParameters.EndDate));
            }

            if (exitQueryParameters.VehicleId != 0 && exitQueryParameters.VehicleLicensePlate is null)
            {
                exitQuery = exitQuery.Where(exit => exit.Entry.VehicleId == exitQueryParameters.VehicleId);
            }
            else if (exitQueryParameters.VehicleId <= 0 && exitQueryParameters.VehicleLicensePlate is not null)
            {
                exitQuery = exitQuery.Where(exit => exit.Entry.Vehicle.VehicleLicensePlate == exitQueryParameters.VehicleLicensePlate.RemoveSpecialCharacters().ToUpper());
            }
            else if (exitQueryParameters.VehicleId != 0 && exitQueryParameters.VehicleLicensePlate is not null)
            {
                exitQuery = exitQuery.Where(exit => exit.Entry.Vehicle.VehicleLicensePlate == exitQueryParameters.VehicleLicensePlate.RemoveSpecialCharacters().ToUpper() &&
                                                    exit.Entry.VehicleId == exitQueryParameters.VehicleId);
            }
            exitQuery = exitQuery.AsNoTracking().Include(entry => entry.Entry)
                                                  .ThenInclude(vehicle => vehicle.Vehicle)
                                                  .ThenInclude(customer => customer.Customer);
            var exitsMapped = await PagedListService<ExitResponseDto, Exit>.CreatePagedList(exitQuery, exitQueryParameters, _mapper);
            if (!exitsMapped.Any())
            {
                return ResponseBase<PagedList<ExitResponseDto>>.FailureResult("Não há saidas", HttpStatusCode.NotFound);
            }
            return ResponseBase<PagedList<ExitResponseDto>>.SuccessResult(exitsMapped, "Saidas encontradas.");
        }

        public async Task<ResponseBase<ExitResponseDto>> GetExitById(int exitId)
        {
            var exitSearched = await _unitOfWork.ExitRepository.GetAllQueryable()
                                                           .AsNoTracking()
                                                           .Where(exit => exit.ExitId == exitId)
                                                           .Include(entry => entry.Entry)
                                                           .ThenInclude(vehicle => vehicle.Vehicle)
                                                           .ThenInclude(customer => customer.Customer)
                                                           .FirstOrDefaultAsync();
            if (exitSearched == null)
            {
                return ResponseBase<ExitResponseDto>.FailureResult("Não há entrada com esse ID", HttpStatusCode.NotFound);
            }
            return ResponseBase<ExitResponseDto>.SuccessResult(_mapper.Map<ExitResponseDto>(exitSearched), "Saida encontrada");

        }

        public async Task<ResponseBase<ExitResponseDto>> UpdateExitDate(ExitUpdateRequestDto exitUpdateRequestDto)
        {
            var exitSearched = await _unitOfWork.ExitRepository.GetAllQueryable()
                                                            .Where(exit => exit.ExitId == exitUpdateRequestDto.ExitId)
                                                            .Include(entry => entry.Entry)
                                                            .ThenInclude(Vehicle => Vehicle.Vehicle)
                                                            .ThenInclude(customer => customer.Customer)
                                                            .FirstOrDefaultAsync();
            if (exitSearched is null)
            {
                return ResponseBase<ExitResponseDto>.FailureResult("Saida não encontrada.", HttpStatusCode.NoContent);
            }

            if (exitUpdateRequestDto.ExitDate < exitSearched.Entry.EntryDate)
            {
                return ResponseBase<ExitResponseDto>.FailureResult($"Data de saida {exitUpdateRequestDto.ExitDate} esta maior que a data de entrada {exitSearched.Entry.EntryDate}"
                                                                                    , HttpStatusCode.BadRequest);
            }

            var chargedAmount = await _unitOfWork.ExitRepository
                                    .SearchVehicleAndCalculateTheAmountPay(exitSearched.Entry.VehicleId, exitSearched.Entry.EntryDate, exitUpdateRequestDto.ExitDate);

            exitSearched.ChargedAmount = chargedAmount;
            exitSearched.ExitDate = exitUpdateRequestDto.ExitDate;
            _unitOfWork.ExitRepository.UpdateAsync(exitSearched);
            await _unitOfWork.Commit();
            return ResponseBase<ExitResponseDto>.SuccessResult(_mapper.Map<ExitResponseDto>(exitSearched), "Saida encontrada");

        }
    }
}
