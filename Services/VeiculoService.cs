using AutoMapper;
using Estacionei.DTOs;
using Estacionei.DTOs.Cliente;
using Estacionei.DTOs.Veiculo;
using Estacionei.DTOs.Veiculos;
using Estacionei.Enums;
using Estacionei.Extensions;
using Estacionei.Models;
using Estacionei.Pagination;
using Estacionei.Pagination.Parameters.Veiculo;
using Estacionei.Repository.Interfaces;
using Estacionei.Response;
using Estacionei.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net;
using System.Numerics;

namespace Estacionei.Services
{
    public class VeiculoService : IVeiculoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public VeiculoService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ResponseBase<PagedList<VeiculoResponseDto>>> GetAllAsync(VeiculoQueryParameters queryParameters)
        {
            var veiculosQueryable = _unitOfWork.VeiculoRepository.GetAllQueryable();

            if (queryParameters.ClienteId != 0)
            {
                veiculosQueryable = veiculosQueryable.Where(veiculo => veiculo.ClienteId == queryParameters.ClienteId);
            }
            if (!string.IsNullOrEmpty(queryParameters.ClienteNome))
            {
                veiculosQueryable = veiculosQueryable.Where(veiculo => veiculo.Cliente.ClienteNome.ToUpper() == queryParameters.ClienteNome.RemoveSpecialCharacters().ToUpper());
            }
            if (queryParameters.TipoVeiculo != 0)
            {
                veiculosQueryable = veiculosQueryable.Where(veiculo => veiculo.TipoVeiculo == queryParameters.TipoVeiculo);
            }
            veiculosQueryable = veiculosQueryable.Include(cliente => cliente.Cliente);
            var veiculosPaginados = await PagedListService<VeiculoResponseDto, Veiculo>.CreatePagedList(veiculosQueryable, queryParameters, _mapper);

            if (!veiculosPaginados.Any())
            {
                return ResponseBase<PagedList<VeiculoResponseDto>>.FailureResult("Não há veiculos cadastrados com os parametros passados.", HttpStatusCode.NotFound);
            }
            return ResponseBase<PagedList<VeiculoResponseDto>>.SuccessResult(veiculosPaginados, "Veiculos encontrados");

        }



        public async Task<ResponseBase<VeiculoResponseDto>> GetVeiculoByIdAsync(int id)
        {
            var veiculo = await _unitOfWork.VeiculoRepository.GetAllQueryable().Where(x => x.VeiculoId == id).Include(cliente => cliente.Cliente).FirstOrDefaultAsync();
            if (veiculo == null)
            {
                return ResponseBase<VeiculoResponseDto>.FailureResult("Veiculo não encontrado.", HttpStatusCode.NotFound);
            }
            return ResponseBase<VeiculoResponseDto>.SuccessResult(_mapper.Map<VeiculoResponseDto>(veiculo), "Veiculo encontrado");

        }

        public async Task<ResponseBase<VeiculoResponseDto>> GetVeiculoByPlacaAsync(string placa)
        {
            var formattedPlate = placa.RemoveSpecialCharacters().ToUpper();
            var veiculo = await _unitOfWork.VeiculoRepository.GetAllQueryable()
                                                            .Include(veiculo => veiculo.Cliente)
                                                            .Where(v => v.VeiculoPlaca == formattedPlate)
                                                            .FirstOrDefaultAsync();

            if (veiculo == null)
            {
                return ResponseBase<VeiculoResponseDto>.FailureResult("Veiculo não encontrado.", HttpStatusCode.NotFound);

            }
            return ResponseBase<VeiculoResponseDto>.SuccessResult(_mapper.Map<VeiculoResponseDto>(veiculo), "Veiculo encontrado");

        }


        public async Task<ResponseBase<VeiculoResponseDto>> CreateVehicle(VeiculoRequestDto veiculoCreateDto)
        {
            var vehicleExists = await CheckPlate(veiculoCreateDto.VeiculoPlaca);

            if (vehicleExists)
            {
                return ResponseBase<VeiculoResponseDto>.FailureResult("Placa ja existe no banco de dados.", HttpStatusCode.BadRequest);
            }
            var resultCliente = await GetCliente(veiculoCreateDto.ClienteId);
            if (resultCliente is null)
            {
                return ResponseBase<VeiculoResponseDto>.FailureResult("Cliente não existe.", HttpStatusCode.NotFound);
            };

            var veiculo = _mapper.Map<Veiculo>(veiculoCreateDto);
            veiculo.VeiculoPlaca = veiculo.VeiculoPlaca.Replace(" ", "").ToUpper().RemoveSpecialCharacters();
            await _unitOfWork.VeiculoRepository.AddAsync(veiculo);
            await _unitOfWork.Commit();
            await _unitOfWork.Dispose();
            var vehicleMapped = _mapper.Map<VeiculoResponseDto>(veiculo);
            vehicleMapped.ClienteNome = resultCliente.ClienteNome;
            return ResponseBase<VeiculoResponseDto>.SuccessResult(vehicleMapped, "veiculo cadastrado com sucesso");

        }

        public async Task<ResponseBase<bool>> UpdateVeiculoAsync(VeiculoRequestDto veiculoUpdateDto)
        {
            var result = await _unitOfWork.VeiculoRepository.GetAsync(x => x.VeiculoId == veiculoUpdateDto.VeiculoId);
            if (result == null)
            {
                return ResponseBase<bool>.FailureResult("Veiculo nao encontrado no banco de dados.", HttpStatusCode.BadRequest);
            }
            var vehicleExists = await CheckPlate(veiculoUpdateDto.VeiculoPlaca);
            if (vehicleExists)
            {
                return ResponseBase<bool>.FailureResult("Placa ja existe no banco de dados.", HttpStatusCode.BadRequest);
            }
            var clienteExist = await GetCliente(veiculoUpdateDto.ClienteId);
            if (clienteExist is null)
            {
                return ResponseBase<bool>.FailureResult("Cliente não existe no banco de dados.", HttpStatusCode.BadRequest);

            }
            veiculoUpdateDto.VeiculoPlaca = veiculoUpdateDto.VeiculoPlaca.RemoveSpecialCharacters();
            _unitOfWork.VeiculoRepository.UpdateAsync(_mapper.Map<Veiculo>(veiculoUpdateDto));
            await _unitOfWork.Commit();
            await _unitOfWork.Dispose();
            return ResponseBase<bool>.SuccessResult(true, "veiculo atualizado com sucesso");

        }

        public async Task<ResponseBase<bool>> DeleteVeiculoAsync(int id)
        {
            var veiculo = await _unitOfWork.VeiculoRepository.GetAsync(x => x.VeiculoId == id);
            if (veiculo != null)
            {
                _unitOfWork.VeiculoRepository.DeleteAsync(veiculo);
                await _unitOfWork.Commit();
                await _unitOfWork.Dispose();
                return ResponseBase<bool>.SuccessResult(true, "Veiculo removido", HttpStatusCode.OK);
            }
            return ResponseBase<bool>.FailureResult("Veiculo nao existe no banco de dados", HttpStatusCode.BadRequest);

        }
        public async Task<bool> CheckPlate(string placa)
        {
            var veiculo = await _unitOfWork.VeiculoRepository.GetAsync(x => x.VeiculoPlaca == placa.ToUpper().Replace(" ", ""));

            return veiculo != null;

        }
        public async Task<bool> CheckPlate(Veiculo veiculo)
        {
            var checkVeiculo = await _unitOfWork.VeiculoRepository.GetAsync(x => x.VeiculoPlaca == veiculo.VeiculoPlaca.ToUpper().Replace(" ", ""));
            if (checkVeiculo != null && (checkVeiculo.VeiculoId != veiculo.VeiculoId))
            {
                return true;
            }

            return false;

        }
        private async Task<Cliente> GetCliente(int id)
        {
            return await _unitOfWork.ClienteRepository.GetAsync(x => x.ClienteId == id);

        }
        public bool ValidateTypeVehicle(int tipoVeiculo)
        {
            return Enum.IsDefined(typeof(TipoVeiculo), tipoVeiculo);
        }


    }
}
