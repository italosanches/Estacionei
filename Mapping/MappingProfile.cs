using AutoMapper;
using Estacionei.DTOs;
using Estacionei.Models;

namespace Estacionei.Mapping
{
	public class MappingProfile : Profile
	{
		public MappingProfile() 
		{
			CreateMap<ClienteCreateDto, Cliente>();
			CreateMap<Cliente, ClienteGetDto>();

			CreateMap<VeiculoDto, Veiculo>();
		}
	}
}
