using AutoMapper;
using Estacionei.DTOs.Cliente;
using Estacionei.Enums;
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

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var result = await _clienteService.GetAllClienteAsync();
			return StatusCode((int)result.StatusCode, (result.Data));
		}

		[HttpGet("{id:int}",Name = "GetById")]
		public async Task<IActionResult> GetById(int id)
		{

			var result = await _clienteService.GetClienteByIdAsync(id);
			if(result.Success)
			{
				return StatusCode((int)result.StatusCode, result.Data);
			}
			else
			{
				return StatusCode((int)result.StatusCode, result.Message);
			}
		}

		[HttpPost]
		public async Task<IActionResult> Create(ClienteRequestCreateDto clientedto)
		{
			if (clientedto.Veiculo is not null)
			{
                bool validateEnum = Enum.IsDefined(typeof(TipoVeiculo), clientedto.Veiculo.TipoVeiculo);
                if (!validateEnum)
                {
                    return BadRequest("Tipo do veiculo é invalido");
                }

            }
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

		[HttpPut("{id:int}")]
		public async Task<IActionResult> Update(int id,ClienteRequestUpdateDto clientedto)
		{
			if(id <= 0)
			{
				return BadRequest("ID invalido");
			}
			clientedto.ClienteId = id;
			var result = await _clienteService.UpdateClienteAsync(clientedto);
			return StatusCode((int)result.StatusCode, result.Message);
		}

		[HttpDelete("{id:int}")]
		public async Task<IActionResult> Delete (int id)
		{
			var result = await _clienteService.DeleteClienteAsync(id);
			return StatusCode((int)result.StatusCode, result.Message);
		}
	}
}
