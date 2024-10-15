using AutoMapper;
using Estacionei.DTOs.Cliente;
using Estacionei.Enums;
using Estacionei.Models;
using Estacionei.Pagination;
using Estacionei.Pagination.Parameters;
using Estacionei.Pagination.Parameters.ClienteParameters;
using Estacionei.Response;
using Estacionei.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace Estacionei.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class ClientesController : ControllerBase
	{
		private readonly IClienteService _clienteService;

		public ClientesController(IClienteService clienteService)
		{
			_clienteService = clienteService;
		}
		[Authorize]
		[HttpGet]
		public async Task<IActionResult> GetAll([FromQuery] ClienteQueryParameters queryParameters)
		{
			var resultPaginated = await _clienteService.GetAllClienteByPaginationAsync(queryParameters);

			return GetClientes(resultPaginated);
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


		private IActionResult GetClientes (ResponseBase<PagedList<ClienteResponseDto>> resultPaginated)
		{
            var metadata = new
            {
                resultPaginated.Data.TotalCount,
                resultPaginated.Data.PageSize,
                resultPaginated.Data.CurrentPage,
                resultPaginated.Data.TotalPages,
                resultPaginated.Data.HasNext,
                resultPaginated.Data.HasPrevious,
            };
            Response.Headers.Append("X-Paginations", JsonConvert.SerializeObject(metadata));
            return StatusCode((int)resultPaginated.StatusCode, resultPaginated.Data);
        }
	}
}
