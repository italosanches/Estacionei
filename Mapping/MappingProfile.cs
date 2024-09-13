using AutoMapper;
using Estacionei.DTOs;
using Estacionei.DTOs.Cliente;
using Estacionei.DTOs.ClienteVeiculo;
using Estacionei.DTOs.ConfiguracaoValorHora;
using Estacionei.DTOs.Veiculo;
using Estacionei.DTOs.Veiculos;
using Estacionei.Models;

namespace Estacionei.Mapping
{
    public class MappingProfile : Profile
	{
		public MappingProfile() 
		{
		    //Cliente
            CreateMap<ClienteRequestCreateDto, Cliente>();
			CreateMap<Cliente, ClienteResponseDto>();
			CreateMap<ClienteRequestUpdateDto, Cliente>();

			//Veiculos
			CreateMap<VeiculoRequestCreateDto, Veiculo>();
			CreateMap<VeiculoRequestUpdateDto, Veiculo>();
			CreateMap<Veiculo, VeiculoGetDto>();

			//Veiculos Clientes
            
			CreateMap<VeiculoClienteDto, Veiculo>().ReverseMap();

         



            //Configuracao Valor Hora
            CreateMap<ConfiguracaoValorHoraCreateDto,ConfiguracaoValorHora>();
            CreateMap<ConfiguracaoValorHoraUpdateDto, ConfiguracaoValorHora>();
            CreateMap<ConfiguracaoValorHora,ConfiguracaoValorHoraGetDto>();


		}
	}
}
