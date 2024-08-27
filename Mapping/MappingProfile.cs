using AutoMapper;
using Estacionei.DTOs;
using Estacionei.DTOs.Cliente;
using Estacionei.Models;

namespace Estacionei.Mapping
{
    public class MappingProfile : Profile
	{
		public MappingProfile() 
		{
			CreateMap<ClienteCreateDto, Cliente>();
			CreateMap<Cliente, ClienteGetDto>();
			CreateMap<ClienteUpdateDto, Cliente>();

			CreateMap<VeiculoDto, Veiculo>();
		}
	}
}
