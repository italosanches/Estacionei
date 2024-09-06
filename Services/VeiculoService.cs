using AutoMapper;
using Estacionei.DTOs;
using Estacionei.DTOs.Veiculos;
using Estacionei.Extensions;
using Estacionei.Models;
using Estacionei.Repository;
using Estacionei.Repository.Interfaces;
using Estacionei.Response;
using Estacionei.Services.Interfaces;
using System.Net;
using System.Numerics;

namespace Estacionei.Services
{
    public class VeiculoService : IVeiculoService
    {
        private readonly IVeiculoRepository _veiculoRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IMapper _mapper;


        public VeiculoService(IVeiculoRepository veiculoRepository, IClienteRepository clienteRepository, IMapper mapper)
        {
            _veiculoRepository = veiculoRepository;
            _clienteRepository = clienteRepository;
            _mapper = mapper;
        }
        public async Task<ResponseBase<IEnumerable<VeiculoGetDto>>> GetAllVeiculoAsync()
        {
            var veiculos = await _veiculoRepository.GetAllAsync();
            if (veiculos.Count() > 0)
            {
                return ResponseBase<IEnumerable<VeiculoGetDto>>.SuccessResult(_mapper.Map<IEnumerable<VeiculoGetDto>>(veiculos), "Lista veiculos", HttpStatusCode.OK);
            }
            else
            {
                return ResponseBase<IEnumerable<VeiculoGetDto>>.FailureResult("Não há veiculos cadastrados", HttpStatusCode.NotFound);
            }

        }

        public async Task<ResponseBase<VeiculoGetDto>> GetVeiculoByIdAsync(int id)
        {
            var veiculo = await _veiculoRepository.GetAsync(x => x.VeiculoId == id);
            if (veiculo == null)
            {
                return ResponseBase<VeiculoGetDto>.FailureResult("Veiculo não encontrado.", HttpStatusCode.NotFound);
            }
            return ResponseBase<VeiculoGetDto>.SuccessResult(_mapper.Map<VeiculoGetDto>(veiculo), "Veiculo encontrado");

        }

        public async Task<ResponseBase<VeiculoGetDto>> GetVeiculoByPlacaAsync(string placa)
        {
            var veiculo = await _veiculoRepository.GetVeiculoByPlaca(placa.ToUpper().RemoveSpecialCharacters().Replace(" ",""));
            if (veiculo == null)
            {
                return ResponseBase<VeiculoGetDto>.FailureResult("Veiculo não encontrado.", HttpStatusCode.NotFound);

            }
            return ResponseBase<VeiculoGetDto>.SuccessResult(_mapper.Map<VeiculoGetDto>(veiculo), "Veiculo encontrado");

        }


        public async Task<ResponseBase<VeiculoGetDto>> AddVeiculoAsync(VeiculoCreateDto veiculoCreateDto)
        {
            var veiculoExists = await CheckPlate(veiculoCreateDto.VeiculoPlaca);

            if (veiculoExists)
            {
                return ResponseBase<VeiculoGetDto>.FailureResult("Placa ja existe no banco de dados.", HttpStatusCode.BadRequest);
            }
            var resultCliente = await ClienteExists(veiculoCreateDto.ClienteId);
            if (!resultCliente)
            {
                return ResponseBase<VeiculoGetDto>.FailureResult("Cliente não existe.", HttpStatusCode.NotFound);
            };

            var veiculo = _mapper.Map<Veiculo>(veiculoCreateDto);
            veiculo.VeiculoPlaca = veiculo.VeiculoPlaca.Replace(" ","").ToUpper().RemoveSpecialCharacters();
            await _veiculoRepository.AddAsync(veiculo);
            return ResponseBase<VeiculoGetDto>.SuccessResult(_mapper.Map<VeiculoGetDto>(veiculo), "veiculo cadastrado com sucesso");

        }
        //Adicionar veiculo ao cadastrar cliente
        public async Task<ResponseBase<VeiculoGetDto>> AddClienteVeiculoAsync(VeiculoCreateDto veiculoCreateDto)
        {
            var veiculo = _mapper.Map<Veiculo>(veiculoCreateDto);
            veiculo.VeiculoPlaca = veiculo.VeiculoPlaca.RemoveSpecialCharacters().Replace(" ","").ToUpper();
            await _veiculoRepository.AddAsync(veiculo);
            return ResponseBase<VeiculoGetDto>.SuccessResult(_mapper.Map<VeiculoGetDto>(veiculo), "veiculo cadastrado com sucesso");

        }
        public async Task<ResponseBase<bool>> UpdateVeiculoAsync(VeiculoUpdateDto veiculoUpdateDto)
        {
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
            await _veiculoRepository.UpdateAsync(_mapper.Map<Veiculo>(veiculoUpdateDto));
            return ResponseBase<bool>.SuccessResult(true, "veiculo atualizado com sucesso");

        }

        public async Task<ResponseBase<bool>> DeleteVeiculoAsync(int id)
        {
            var veiculo = await _veiculoRepository.GetAsync(x => x.VeiculoId == id);
            if (veiculo != null)
            {
                await _veiculoRepository.DeleteAsync(veiculo);
                return ResponseBase<bool>.SuccessResult(true, "Veiculo removido", HttpStatusCode.OK);
            }
            return ResponseBase<bool>.FailureResult("Veiculo nao existe no banco de dados", HttpStatusCode.BadRequest);

        }
        public async Task<bool> CheckPlate(string placa)
        {
            var veiculo = await _veiculoRepository.GetAsync(x => x.VeiculoPlaca == placa.ToUpper().Replace(" ", ""));

            return veiculo != null;

        }
        public async Task<bool> CheckPlate(Veiculo veiculo)
        {
            var checkVeiculo = await _veiculoRepository.GetAsync(x => x.VeiculoPlaca == veiculo.VeiculoPlaca.ToUpper().Replace(" ", ""));
            if (checkVeiculo != null && (checkVeiculo.VeiculoId != veiculo.VeiculoId))
            {
                return true;
            }

            return false;

        }
        private async Task<bool> ClienteExists(int id)
        {
            var cliente = await _clienteRepository.GetAsync(x=> x.ClienteId == id);
            return cliente != null;
        }

        
    }
}
