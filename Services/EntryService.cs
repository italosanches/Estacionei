using AutoMapper;
using Estacionei.DTOs.Entrada;
using Estacionei.Enums;
using Estacionei.Extensions;
using Estacionei.Models;
using Estacionei.Pagination;
using Estacionei.Pagination.Parameters.EntradaParameters;
using Estacionei.Repository.Interfaces;
using Estacionei.Response;
using Estacionei.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Estacionei.Services
{
    public class EntryService : IEntryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EntryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ResponseBase<PagedList<EntryResponseDto>>> GetAllEntries(EntryQueryParameters queryParameters)
        {
            var entryQueryable = _unitOfWork.EntryRepository.GetAllQueryable();

            if (queryParameters.EndDate != DateTime.MinValue && queryParameters.StartDate > queryParameters.EndDate)
            {
                return ResponseBase<PagedList<EntryResponseDto>>.FailureResult("Data de início não pode ser maior que a data de fim", HttpStatusCode.BadRequest);
            }
            else
            {   //Filtranda pela data inicio e fim, caso nao seja passado nenhum valor = min value, entao o filtro nao é aplicado
                entryQueryable = entryQueryable.Where(entry => (queryParameters.StartDate == DateTime.MinValue || entry.EntryDate >= queryParameters.StartDate) &&
                                                               (queryParameters.EndDate == DateTime.MinValue || entry.EntryDate <= queryParameters.EndDate));
            }
           
            if (queryParameters.VehicleId != 0 && queryParameters.VehicleLicensePlate is null)
            {
                entryQueryable = entryQueryable.Where(entry => entry.VehicleId == queryParameters.VehicleId);
            }
            else if (queryParameters.VehicleId <= 0 && queryParameters.VehicleLicensePlate is not null)
            {
                entryQueryable = entryQueryable.Where(entry => entry.Vehicle.VehicleLicensePlate == queryParameters.VehicleLicensePlate.RemoveSpecialCharacters().ToUpper());
            }
            else if (queryParameters.VehicleId != 0 && queryParameters.VehicleLicensePlate is not null)
            {
                entryQueryable = entryQueryable.Where(entry => entry.Vehicle.VehicleLicensePlate == queryParameters.VehicleLicensePlate.RemoveSpecialCharacters().ToUpper() &&
                                                                       entry.VehicleId == queryParameters.VehicleId);
            }
            entryQueryable = entryQueryable.Include(entrie => entrie.Vehicle)
                                           .ThenInclude(vehicle => vehicle.Customer);

            var entriesPagedList = await PagedListService<EntryResponseDto, Entry>.CreatePagedList(entryQueryable, queryParameters, _mapper);
            if (!entriesPagedList.Any())
            {
                return ResponseBase<PagedList<EntryResponseDto>>.FailureResult("Não há entradas cadastradas", HttpStatusCode.NotFound);
            }
            //var entriesDto = _mapper.Map<IEnumerable<EntryQueryParameters>>(entryQueryable);
            return ResponseBase<PagedList<EntryResponseDto>>.SuccessResult(entriesPagedList, "Entradas encontradas");

        }

        public async Task<ResponseBase<EntryResponseDto>> GetEntryById(int id)
        {
            var entry = await _unitOfWork.EntryRepository.GetAllQueryable().Include(entry => entry.Vehicle)
                                                                           .ThenInclude(Vehicle => Vehicle.Customer)
                                                                           .Where(entry => entry.EntryId == id)
                                                                           .FirstOrDefaultAsync();
            if (entry == null)
            {

                return ResponseBase<EntryResponseDto>.FailureResult("Não há entradas com esse id", HttpStatusCode.NotFound);
            }
            var entriesDto = _mapper.Map<EntryResponseDto>(entry);
            return ResponseBase<EntryResponseDto>.SuccessResult(entriesDto, "Entradas encontradas");
        }

        public async Task<ResponseBase<EntryResponseDto>> CreateEntry(EntryRequestDto entryRequestDto)
        {
            var vehicle = await _unitOfWork.VehicleRepository.GetAllQueryable().Where(searchedVehicle => searchedVehicle.VehicleId == entryRequestDto.VehicleId)
                                                                               .Include(x => x.Customer)
                                                                               .FirstOrDefaultAsync();
            if (vehicle is null)
            {
                return ResponseBase<EntryResponseDto>.FailureResult("Veiculo não encontrado.", HttpStatusCode.NotFound);
            }
            if (await _unitOfWork.EntryRepository.HasOpenEntryForVehicle(vehicle.VehicleId))
            {
                return ResponseBase<EntryResponseDto>.FailureResult("Ja existe uma entrada em aberto para esse veiculo.", HttpStatusCode.BadRequest);
            }
            if (entryRequestDto.EntryDate > DateTime.UtcNow)
            {
                return ResponseBase<EntryResponseDto>.FailureResult("Data de entrada deve ser menor que a data de hoje", HttpStatusCode.BadRequest);

            }
            var newEntry = _mapper.Map<Entry>(entryRequestDto);
            await _unitOfWork.EntryRepository.AddAsync(newEntry);
            await _unitOfWork.Commit();
            await _unitOfWork.Dispose();
            newEntry.Vehicle = vehicle;
            var responseEntry = _mapper.Map<EntryResponseDto>(newEntry);
            return ResponseBase<EntryResponseDto>.SuccessResult(responseEntry, "Entrada criada");


        }

        public async Task<ResponseBase<bool>> UpdateAsync(EntryRequestDto entryRequestDto)
        {
            var searchedEntry = await _unitOfWork.EntryRepository.GetAsync(entry => entry.EntryId == entryRequestDto.EntryId);
            if (searchedEntry == null)
            {
                return ResponseBase<bool>.FailureResult("Entrada não encotrada.", HttpStatusCode.NotFound);

            }
            var searchedExit = await _unitOfWork.ExitRepository.GetAsync(entry => entry.EntryId == searchedEntry.EntryId);
            if (searchedExit != null)
            {
                return ResponseBase<bool>.FailureResult($"Existe uma saida vinculada ao Id: {searchedExit.ExitId}, remova antes de alterar a entrada.", HttpStatusCode.Conflict);
            }
            if (await _unitOfWork.VehicleRepository.GetAsync(vehicle => vehicle.VehicleId == entryRequestDto.VehicleId) == null)
            {
                return ResponseBase<bool>.FailureResult("Veiculo não encontrado.", HttpStatusCode.NotFound);
            }
            if (entryRequestDto.EntryDate > DateTime.UtcNow)
            {
                return ResponseBase<bool>.FailureResult("Entrada deve ser menor que a data de hoje", HttpStatusCode.BadRequest);
            }
            if (entryRequestDto.VehicleId != searchedEntry.VehicleId)
            {
                var checkIfVehicleHasOpenEntry = await _unitOfWork.EntryRepository
                                                          .GetAsync(entry => entry.VehicleId == entryRequestDto.VehicleId && entry.EntryStatus == EntryStatus.Open);
                if (checkIfVehicleHasOpenEntry != null)
                {
                    return ResponseBase<bool>.FailureResult($"Existe uma entrada em aberto para o veiculo {checkIfVehicleHasOpenEntry.VehicleId}", HttpStatusCode.Conflict);
                }
            }

            _unitOfWork.EntryRepository.UpdateAsync(_mapper.Map<Entry>(entryRequestDto));
            await _unitOfWork.Commit();
            await _unitOfWork.Dispose();
            return ResponseBase<bool>.SuccessResult(true, "Entrada atualizada");
        }

        public async Task<ResponseBase<bool>> DeleteAsync(int id)
        {
            var searchedEntry = await _unitOfWork.EntryRepository.GetAsync(entry => entry.EntryId == id);
            if (searchedEntry == null)
            {
                return ResponseBase<bool>.FailureResult("Entrada não encotrada.", HttpStatusCode.NotFound);

            }
            var exitSearched = await _unitOfWork.ExitRepository.GetAsync(entry => entry.EntryId == searchedEntry.EntryId);
            if (exitSearched != null)
            {
                return ResponseBase<bool>.FailureResult($"Existe uma saida vinculada.Id: {exitSearched.ExitId}, remova antes de deletar a entrada.", HttpStatusCode.Conflict);
            }
            _unitOfWork.EntryRepository.DeleteAsync(_mapper.Map<Entry>(searchedEntry));
            await _unitOfWork.Commit();
            return ResponseBase<bool>.SuccessResult(true, "Entrada removida.");
        }
    }
}
