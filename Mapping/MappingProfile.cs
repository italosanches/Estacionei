using AutoMapper;
using Estacionei.DTOs;
using Estacionei.DTOs.Cliente;
using Estacionei.DTOs.ClienteVeiculo;
using Estacionei.DTOs.ConfiguracaoValorHora;
using Estacionei.DTOs.Entrada;
using Estacionei.DTOs.Veiculo;
using Estacionei.DTOs.Veiculos;
using Estacionei.Enums;
using Estacionei.Models;

namespace Estacionei.Mapping
{
    public class MappingProfile : Profile
	{
		public MappingProfile() 
		{
		    //Cliente
            CreateMap<ClienteRequestCreateDto, Cliente>();
			CreateMap<Cliente, ClienteResponseDto>().ForMember(dest => dest.VeiculosCliente, opt => opt.MapFrom(src => src.VeiculosCliente)); 
			

			//Veiculos
			CreateMap<VeiculoRequestCreateDto, Veiculo>();
			CreateMap<VeiculoRequestUpdateDto, Veiculo>();
			CreateMap<Veiculo, VeiculoResponseDto>();
            CreateMap<Veiculo, VeiculoClienteResponseDto>();


            //Veiculos Clientes
            CreateMap<VeiculoClienteRequestDto, Veiculo>().ReverseMap();
			CreateMap<Veiculo, VeiculoClienteResponseDto>().ReverseMap();


            //Configuracao Valor Hora
            CreateMap<ConfiguracaoValorHoraCreateDto,ConfiguracaoValorHora>();
            CreateMap<ConfiguracaoValorHoraUpdateDto, ConfiguracaoValorHora>();
            CreateMap<ConfiguracaoValorHora,ConfiguracaoValorHoraGetDto>();

           //Configuracao entrada
            CreateMap<EntradaRequestCreateDto, Entrada>().ForMember(dest => dest.StatusEntrada ,option =>option.MapFrom(src => StatusEntrada.Aberto));
			CreateMap<Entrada, EntradaResponseDto>().ForMember(dest => dest.VeiculoId,option => option.MapFrom(src => src.Veiculo.VeiculoId))
				                                    .ForMember(dest =>dest.VeiculoPlaca, option => option.MapFrom(src=> src.Veiculo.VeiculoPlaca))
													.ForMember(dest=> dest.ClienteId,option=>option.MapFrom(src=> src.Veiculo.Cliente.ClienteId))
													.ForMember(dest => dest.ClienteNome,option => option.MapFrom(src => src.Veiculo.Cliente.ClienteNome));

		}
	}
}
