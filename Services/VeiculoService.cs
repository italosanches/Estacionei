using AutoMapper;
using Estacionei.DTOs;
using Estacionei.DTOs.Cliente;
using Estacionei.DTOs.Veiculo;
using Estacionei.DTOs.Veiculos;
using Estacionei.Extensions;
using Estacionei.Models;
using Estacionei.Repository.Interfaces;
using Estacionei.Response;
using Estacionei.Services.Interfaces;
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
        public async Task<ResponseBase<IEnumerable<VeiculoResponseDto>>> GetAllVeiculoAsync()
        {
            var veiculos = await _unitOfWork.VeiculoRepository.GetAllAsync();
            var veiculosDto = _mapper.Map<IEnumerable<VeiculoResponseDto>>(veiculos);
            if(!veiculosDto.Any())
            {
                return ResponseBase<IEnumerable<VeiculoResponseDto>>.FailureResult("Não há veiculos cadastrados", HttpStatusCode.NotFound);
            }
            return ResponseBase<IEnumerable<VeiculoResponseDto>>.SuccessResult((veiculosDto ?? new List<VeiculoResponseDto>()), "Lista veiculos", HttpStatusCode.OK);

        }
        public async Task<ResponseBase<IEnumerable<VeiculoResponseDto>>> GetAllVeiculoByClienteAsync(int clienteId)
        {
            var veiculos = await _unitOfWork.VeiculoRepository.GetVeiculoByClienteAsync(clienteId);
            if (veiculos.Count() > 0)
            {
                return ResponseBase<IEnumerable<VeiculoResponseDto>>.SuccessResult(_mapper.Map<IEnumerable<VeiculoResponseDto>>(veiculos), "Lista veiculos", HttpStatusCode.OK);
            }
            else
            {
                return ResponseBase<IEnumerable<VeiculoResponseDto>>.FailureResult("Não há veiculos cadastrados para esse cliente", HttpStatusCode.NotFound);
            }
        }


        public async Task<ResponseBase<VeiculoResponseDto>> GetVeiculoByIdAsync(int id)
        {
            var veiculo = await _unitOfWork.VeiculoRepository.GetAsync(x => x.VeiculoId == id);
            if (veiculo == null)
            {
                return ResponseBase<VeiculoResponseDto>.FailureResult("Veiculo não encontrado.", HttpStatusCode.NotFound);
            }
            return ResponseBase<VeiculoResponseDto>.SuccessResult(_mapper.Map<VeiculoResponseDto>(veiculo), "Veiculo encontrado");

        }

        public async Task<ResponseBase<VeiculoResponseDto>> GetVeiculoByPlacaAsync(string placa)
        {
            var veiculo = await _unitOfWork.VeiculoRepository.GetVeiculoByPlacaAsync(placa.ToUpper().RemoveSpecialCharacters().Replace(" ",""));
            if (veiculo == null)
            {
                return ResponseBase<VeiculoResponseDto>.FailureResult("Veiculo não encontrado.", HttpStatusCode.NotFound);

            }
            return ResponseBase<VeiculoResponseDto>.SuccessResult(_mapper.Map<VeiculoResponseDto>(veiculo), "Veiculo encontrado");

        }


        public async Task<ResponseBase<VeiculoResponseDto>> AddVeiculoAsync(VeiculoRequestCreateDto veiculoCreateDto)
        {
            var veiculoExists = await CheckPlate(veiculoCreateDto.VeiculoPlaca);

            if (veiculoExists)
            {
                return ResponseBase<VeiculoResponseDto>.FailureResult("Placa ja existe no banco de dados.", HttpStatusCode.BadRequest);
            }
            var resultCliente = await ClienteExists(veiculoCreateDto.ClienteId);
            if (!resultCliente)
            {
                return ResponseBase<VeiculoResponseDto>.FailureResult("Cliente não existe.", HttpStatusCode.NotFound);
            };

            var veiculo = _mapper.Map<Veiculo>(veiculoCreateDto);
            veiculo.VeiculoPlaca = veiculo.VeiculoPlaca.Replace(" ","").ToUpper().RemoveSpecialCharacters();
            await _unitOfWork.VeiculoRepository.AddAsync(veiculo);
            await _unitOfWork.Commit();
            await _unitOfWork.Dispose();
            return ResponseBase<VeiculoResponseDto>.SuccessResult(_mapper.Map<VeiculoResponseDto>(veiculo), "veiculo cadastrado com sucesso");

        }

        public async Task<ResponseBase<bool>> UpdateVeiculoAsync(VeiculoRequestUpdateDto veiculoUpdateDto)
        {
            var result = await _unitOfWork.VeiculoRepository.GetAsync(x => x.VeiculoId == veiculoUpdateDto.VeiculoId);
            if(result == null)
            {
                return ResponseBase<bool>.FailureResult("Veiculo nao encontrado no banco de dados.", HttpStatusCode.BadRequest);
            }
            var veiculoExists = await CheckPlate(_mapper.Map<Veiculo>(veiculoUpdateDto));
            if (veiculoExists)
            {
                return ResponseBase<bool>.FailureResult("Placa ja existe no banco de dados.", HttpStatusCode.BadRequest);
            }
            var clienteExist = await ClienteExists(veiculoUpdateDto.ClienteId);
            if(!clienteExist)
            {
                return ResponseBase<bool>.FailureResult("Cliente não existe no banco de dados.", HttpStatusCode.BadRequest);

            }
            veiculoUpdateDto.VeiculoPlaca = veiculoUpdateDto.VeiculoPlaca.RemoveSpecialCharacters();
            await _unitOfWork.VeiculoRepository.UpdateAsync(_mapper.Map<Veiculo>(veiculoUpdateDto));
            await _unitOfWork.Commit();
            await _unitOfWork.Dispose();
            return ResponseBase<bool>.SuccessResult(true, "veiculo atualizado com sucesso");

        }

        public async Task<ResponseBase<bool>> DeleteVeiculoAsync(int id)
        {
            var veiculo = await _unitOfWork.VeiculoRepository.GetAsync(x => x.VeiculoId == id);
            if (veiculo != null)
            {
                await _unitOfWork.VeiculoRepository.DeleteAsync(veiculo);
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
        private async Task<bool> ClienteExists(int id)
        {
            var cliente = await _unitOfWork.ClienteRepository.GetAsync(x=> x.ClienteId == id);
            return cliente != null;
        }

       
    }
}
