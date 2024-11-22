using AutoMapper;
using Estacionei.DTOs;
using Estacionei.DTOs.Account;
using Estacionei.DTOs.Cliente;
using Estacionei.DTOs.ClienteVeiculo;
using Estacionei.DTOs.ConfiguracaoValorHora;
using Estacionei.DTOs.Customer;
using Estacionei.DTOs.Entrada;
using Estacionei.DTOs.Saida;
using Estacionei.DTOs.Vehicle;
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
            CreateMap<CustomerRequestCreateDto, Customer>().ReverseMap();
			CreateMap<Customer, CustomerResponseDto>().ForMember(dest => dest.CustomerVehicles, opt => opt.MapFrom(src => src.CustomerVehicles)).ReverseMap();
			CreateMap<CustomerRequestUpdateDto, Customer>().ReverseMap();


			//Veiculos
			CreateMap<VehicleRequestDto, Vehicle>();
			CreateMap<Vehicle, VehicleResponseDto>().ForMember(dest => dest.CustomerName , option => option.MapFrom(src => src.Customer.CustomerName));
            CreateMap<Vehicle, CustomerVehicleResponseDto>();


            //Veiculos Clientes
            CreateMap<CustomerVehicleRequestDto, Vehicle>().ReverseMap();
			CreateMap<Vehicle, CustomerVehicleResponseDto>().ReverseMap();


            //Configuracao Valor Hora
            CreateMap<HourPriceConfigurationRequestDto,HourPriceConfiguration>().ReverseMap();
            CreateMap<HourPriceConfiguration, HourPriceConfigurationResponseDto>();

           //Configuracao entrada
			CreateMap<EntryRequestDto, Entry>().ForMember(dest => dest.EntryStatus, option => option.MapFrom(src => EntryStatus.Open)); ;
			CreateMap<Entry, EntryResponseDto>().ForMember(dest => dest.VehicleId, option => option.MapFrom(src => src.VehicleId))
												.ForMember(dest => dest.VehicleLicensePlate, option => option.MapFrom(src => src.Vehicle.VehicleLicensePlate))
												.ForMember(dest => dest.VehicleModel, option => option.MapFrom(src => src.Vehicle.VehicleModel))
												.ForMember(dest => dest.CustomerId, option => option.MapFrom(src => src.Vehicle.Customer.CustomerId))
												.ForMember(dest => dest.CustomerName, option => option.MapFrom(src => src.Vehicle.Customer.CustomerName));
													

			//Configuracao saida
			CreateMap<ExitRequestDto, Exit>().ForMember(dest => dest.ChargedAmount, option => option.Ignore());
			CreateMap<Exit, ExitResponseDto>().ForMember(dest => dest.EntryDate, option => option.MapFrom(src => src.Entry.EntryDate))
												.ForMember(dest => dest.VehicleId, option => option.MapFrom(src => src.Entry.VehicleId))
												.ForMember(dest => dest.VehicleLicensePlate, option => option.MapFrom(src => src.Entry.Vehicle.VehicleLicensePlate))
												.ForMember(dest => dest.CustomerId, option => option.MapFrom(src => src.Entry.Vehicle.Customer.CustomerId))
												.ForMember(dest => dest.CustomerName, option => option.MapFrom(src => src.Entry.Vehicle.Customer.CustomerName))
												.ForMember(dest => dest.VehicleModel, option => option.MapFrom(src => src.Entry.Vehicle.VehicleModel));


			// Users

			CreateMap<ApplicationUser, UserResponseDto>().ForMember(dest => dest.UserId, option => option.MapFrom(src => src.Id))
													     .ForMember(dest => dest.UserEmail, option => option.MapFrom(src => src.Email));

        }
    }
}
