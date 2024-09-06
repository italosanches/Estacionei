using Estacionei.DTOs;
using Estacionei.DTOs.Veiculos;
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
    public class VeiculoController : ControllerBase
    {
        private readonly IVeiculoService _veiculoService;


        public VeiculoController(IVeiculoService veiculoService)
        {
            _veiculoService = veiculoService;
        }
        [HttpGet("Veiculos")]
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

        [HttpPost]
        public async Task<IActionResult> Create(VeiculoCreateDto veiculoCreateDto)
        {
            var result = await _veiculoService.AddVeiculoAsync(veiculoCreateDto);
            if (result.Success)
            {
                return CreatedAtAction(nameof(GetById), new { id = result.Data.VeiculoId }, result.Data);
            }
            return StatusCode((int)result.StatusCode, result.Message);
        }

        [HttpPut]
        public async Task<IActionResult> Update(VeiculoUpdateDto veiculoUpdateDto)
        {
            var result = await _veiculoService.UpdateVeiculoAsync(veiculoUpdateDto);
            return StatusCode((int)result.StatusCode, result.Message);
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delet(int id)
        {
            var result = await _veiculoService.DeleteVeiculoAsync(id);
            return StatusCode((int)result.StatusCode, result.Message);
        }
    }
}
