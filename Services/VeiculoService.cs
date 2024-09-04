using AutoMapper;
using Estacionei.DTOs;
using Estacionei.DTOs.Veiculos;
using Estacionei.Models;
using Estacionei.Repository.Interfaces;
using Estacionei.Response;
using Estacionei.Services.Interfaces;
using System.Net;

namespace Estacionei.Services
{
    public class VeiculoService : IVeiculoService
    {
        private readonly IVeiculoRepository _veiculoRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IMapper _mapper;

      
        public VeiculoService(IVeiculoRepository veiculoRepository, IClienteRepository clienteRepository,IMapper mapper)
        {
            _veiculoRepository = veiculoRepository;
            _clienteRepository = clienteRepository;
            _mapper            = mapper;
        }
		public Task<IEnumerable<Veiculo>> GetAllVeiculoAsync()
		{
			throw new NotImplementedException();
		}

		public async Task<ResponseBase<VeiculoGetDto>> GetVeiculoByIdAsync(int id)
		{
		    var veiculo = await _veiculoRepository.GetByIdAsync(id);
            if (veiculo == null)
            {
                return ResponseBase<VeiculoGetDto>.FailureResult("Veiculo não encontrado.",HttpStatusCode.NotFound);
            }
            return ResponseBase<VeiculoGetDto>.SuccessResult(_mapper.Map<VeiculoGetDto>(veiculo),"Veiculo encontrado");

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
        public Task<ResponseBase<bool>> UpdateVeiculoAsync(VeiculoUpdateDto veiculoUpdateDto)
		{
			throw new NotImplementedException();
		}

        public Task<ResponseBase<bool>> DeleteVeiculoAsync(int id)
        {
            throw new NotImplementedException();
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
