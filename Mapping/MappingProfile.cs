using AutoMapper;
using Estacionei.DTOs;
using Estacionei.DTOs.Cliente;
using Estacionei.DTOs.Veiculos;
using Estacionei.Models;

namespace Estacionei.Mapping
{
    public class MappingProfile : Profile
	{
		public MappingProfile() 
		{
			//Cliente
			CreateMap<ClienteCreateDto, Cliente>();
			CreateMap<Cliente, ClienteGetDto>();
			CreateMap<ClienteUpdateDto, Cliente>();

			//Veiculos
			CreateMap<VeiculoCreateDto, Veiculo>();
			CreateMap<VeiculoUpdateDto, Veiculo>();
			CreateMap<Veiculo, VeiculoGetDto>().ForMember(dest => dest.Cliente,opt => opt.MapFrom(src => src.Cliente));

		}
	}
}
