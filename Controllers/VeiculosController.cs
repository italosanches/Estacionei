using Estacionei.DTOs;
using Estacionei.DTOs.Veiculo;
using Estacionei.DTOs.Veiculos;
using Estacionei.Enums;
using Estacionei.Models;
using Estacionei.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.RegularExpressions;

namespace Estacionei.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VeiculosController : ControllerBase
    {
        private readonly IVeiculoService _veiculoService;


        public VeiculosController(IVeiculoService veiculoService)
        {
            _veiculoService = veiculoService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _veiculoService.GetAllVeiculoAsync();
            if (result.Success)
            {
                return StatusCode((int)result.StatusCode, result.Data);
            }
            else
            {
                return StatusCode((int)result.StatusCode, result.Message);
            }
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _veiculoService.GetVeiculoByIdAsync(id);
            if (result.Success)
            {
                return StatusCode((int)result.StatusCode, result.Data);
            }
            else
            {
                return StatusCode((int)result.StatusCode, result.Message);
            }
        }

        [HttpGet("{placa}")]
        public async Task<IActionResult> GetByPlaca(string placa)
        {
            if (!Regex.IsMatch(placa, "^[a-zA-Z0-9]+$"))
            {
                return BadRequest("Placa invalida, a placa deve conter apenas letras e numeros!");
            }
            var result = await _veiculoService.GetVeiculoByPlacaAsync(placa);
            if (result.Success)
            {
                return StatusCode((int)result.StatusCode, result.Data);
            }
            else
            {
                return StatusCode((int)result.StatusCode, result.Message);
            }
        }

        [HttpGet("cliente/{id:int}")]
        public async Task<IActionResult> GetByCliente(int id)
        {
            if(id <=0)
            {
                return BadRequest("ID cliente invalido");
            }
            var result = await _veiculoService.GetAllVeiculoByClienteAsync(id);
            if (result.Success)
            {
                return StatusCode((int)result.StatusCode, result.Data);
            }
            else
            {
                return StatusCode((int)result.StatusCode, result.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(VeiculoRequestCreateDto veiculoCreateDto)
        {
            var result = await _veiculoService.AddVeiculoAsync(veiculoCreateDto);
            if (result.Success)
            {
                return CreatedAtAction(nameof(GetById), new { id = result.Data.VeiculoId }, result.Data);
            }
            return StatusCode((int)result.StatusCode, result.Message);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, VeiculoRequestUpdateDto veiculoUpdateDto)
        {
            if (id <= 0)
            {
                return BadRequest("ID do veiculo invalido.");
            }
            veiculoUpdateDto.VeiculoId = id;
            var result = await _veiculoService.UpdateVeiculoAsync(veiculoUpdateDto);
            return StatusCode((int)result.StatusCode, result.Message);
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest("ID do veiculo invalido.");
            }
            var result = await _veiculoService.DeleteVeiculoAsync(id);
            return StatusCode((int)result.StatusCode, result.Message);
        }
    }
}
