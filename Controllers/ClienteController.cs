using AutoMapper;
using Estacionei.DTOs;
using Estacionei.Models;
using Estacionei.Response;
using Estacionei.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Estacionei.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ClienteController : ControllerBase
	{
		private readonly IClienteService _clienteService;
		private readonly IMapper _mapper;

		public ClienteController(IClienteService clienteService, IMapper mapper)
		{
			_clienteService = clienteService;
			_mapper = mapper;
		}
		[HttpGet("{id:int}")]
		public async Task<IActionResult> GetById(int id)
		{

			var result = await _clienteService.GetClienteByIdAsync(id);
			if(result.Success)
			{
				return Ok(result.Data);
			}
			else
			{
				return BadRequest(result.Message);
			}	
		}

		[HttpPost]
		public async Task<IActionResult> Create(ClienteCreateDto clientedto)
		{
			var result = await _clienteService.AddClienteAsync(clientedto);
			if (result.Success)
			{
				return Ok(result);
			}
			else
			{
				return BadRequest(result.Message);
			}

		}
	}
}
