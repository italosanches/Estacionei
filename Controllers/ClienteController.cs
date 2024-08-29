using AutoMapper;
using Estacionei.DTOs.Cliente;
using Estacionei.Models;
using Estacionei.Response;
using Estacionei.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Estacionei.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class ClienteController : ControllerBase
	{
		private readonly IClienteService _clienteService;

		public ClienteController(IClienteService clienteService)
		{
			_clienteService = clienteService;
		}
		[HttpGet("Clientes")]
		public async Task<IActionResult> GetAll()
		{
			var result = await _clienteService.GetAllClienteAsync();
			if (result.Success) 
			{ 
				return StatusCode((int)result.StatusCode,(result.Data));
			}
			else
			{
				return StatusCode((int)result.StatusCode,(result.Message));
			}
		}
		[HttpGet("{id:int}",Name = "GetById")]
		public async Task<IActionResult> GetById(int id)
		{

			var result = await _clienteService.GetClienteByIdAsync(id);
			if(result.Success)
			{
				return StatusCode((int)result.StatusCode, (result.Data));
			}
			else
			{
				return StatusCode((int)result.StatusCode, (result.Message));
			}	
		}

		[HttpPost]
		public async Task<IActionResult> Create(ClienteCreateDto clientedto)
		{
			var result = await _clienteService.AddClienteAsync(clientedto);
			if (result.Success)
			{
				return CreatedAtAction(nameof(GetById), new { id = result.Data.ClienteId }, result.Data);
			}
			else
			{
				return StatusCode((int)result.StatusCode, (result.Message));
			}

		}
	}
}
