using AutoMapper;
using Estacionei.DTOs;
using Estacionei.DTOs.Veiculos;
using Estacionei.Models;
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
                return ResponseBase<IEnumerable<VeiculoGetDto>>.FailureResult("não há veiculos cadastrados", HttpStatusCode.NotFound);
            }

        }

        public async Task<ResponseBase<VeiculoGetDto>> GetVeiculoByIdAsync(int id)
        {
            var veiculo = await _veiculoRepository.GetByIdAsync(id);
            if (veiculo == null)
            {
                return ResponseBase<VeiculoGetDto>.FailureResult("Veiculo não encontrado.", HttpStatusCode.NotFound);
            }
            return ResponseBase<VeiculoGetDto>.SuccessResult(_mapper.Map<VeiculoGetDto>(veiculo), "Veiculo encontrado");

        }

        public async Task<ResponseBase<Veiculo>> AddVeiculoAsync(VeiculoCreateDto veiculoCreateDto)
        {
            var localizarVeiculo = await CheckPlate(veiculoCreateDto.VeiculoPlaca);

            if (localizarVeiculo)
            {
                return ResponseBase<Veiculo>.FailureResult("Placa ja existe no banco de dados.", HttpStatusCode.BadRequest);
            }
            var resultCliente = await ClienteExists(veiculoCreateDto.ClienteId);
            if (!resultCliente)
            {
                return ResponseBase<Veiculo>.FailureResult("Cliente não existe.", HttpStatusCode.NotFound);
            };

            var veiculo = _mapper.Map<Veiculo>(veiculoCreateDto);
            veiculo.VeiculoPlaca = veiculo.VeiculoPlaca.Trim().ToUpper();
            await _veiculoRepository.AddAsync(veiculo);
            return ResponseBase<Veiculo>.SuccessResult(veiculo, "veiculo cadastrado com sucesso");

        }
        //Adicionar veiculo ao cadastrar cliente
        public async Task<ResponseBase<Veiculo>> AddClienteVeiculoAsync(VeiculoCreateDto veiculoCreateDto)
        {
            var veiculo = _mapper.Map<Veiculo>(veiculoCreateDto);
            veiculo.VeiculoPlaca = veiculo.VeiculoPlaca.Trim().ToUpper();
            await _veiculoRepository.AddAsync(veiculo);
            return ResponseBase<Veiculo>.SuccessResult(veiculo, "veiculo cadastrado com sucesso");

        }
        public async Task<ResponseBase<bool>> UpdateVeiculoAsync(VeiculoUpdateDto veiculoUpdateDto)
        {
            var veiculos = await _veiculoRepository.FindAsync(x => x.VeiculoPlaca == veiculoUpdateDto.VeiculoPlaca.ToUpper());
            if (veiculos.Count() > 0)
            {
                foreach (var veiculo in veiculos)
                {
                    if (veiculo.VeiculoId != veiculoUpdateDto.VeiculoId)
                    {
                        return ResponseBase<bool>.FailureResult("Placa ja existe no banco de dados.", HttpStatusCode.BadRequest);
                    }
                }
            }
            await _veiculoRepository.UpdateAsync(_mapper.Map<Veiculo>(veiculoUpdateDto));
            return ResponseBase<bool>.SuccessResult(true, "veiculo atualizado com sucesso");

        }

        public async Task<ResponseBase<bool>> DeleteVeiculoAsync(int id)
        {
            var veiculo = await _veiculoRepository.GetByIdAsync(id);
            if (veiculo != null)
            {
                await _veiculoRepository.DeleteAsync(veiculo);
                return ResponseBase<bool>.SuccessResult(true, "Veiculo removido", HttpStatusCode.OK);
            }
            return ResponseBase<bool>.FailureResult("Veiculo nao existe no banco de dados", HttpStatusCode.BadRequest);

        }
        public async Task<bool> CheckPlate(string placa)
        {
            var listVeiculo = await _veiculoRepository.FindAsync(x => x.VeiculoPlaca == placa.ToUpper());

            return listVeiculo.Count() > 0;
        }
        private async Task<bool> ClienteExists(int id)
        {
            var cliente = await _clienteRepository.GetByIdAsync(id);

            return cliente != null;
        }
    }
}
