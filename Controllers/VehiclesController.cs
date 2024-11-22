using Estacionei.DTOs;
using Estacionei.DTOs.Vehicle;
using Estacionei.DTOs.Veiculos;
using Estacionei.Enums;
using Estacionei.Models;
using Estacionei.Pagination;
using Estacionei.Pagination.Parameters.Vehicle;
using Estacionei.Services;
using Estacionei.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text.RegularExpressions;

namespace Estacionei.Controllers
{
    [Route("api/vehicles")]
    [ApiController]
    [Authorize(Policy = "UserOnly")]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;


        public VehiclesController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery]VehicleQueryParameters vehicleQueryParameters)
        {
            if(vehicleQueryParameters.VehicleType != 0 && (!_vehicleService.ValidateVehicleType((int)vehicleQueryParameters.VehicleType)))
            {
                return BadRequest("Tipo de Vehicle incorreto");
            }

            var result = await _vehicleService.GetAllAsync(vehicleQueryParameters);
            if (result.Success)
            {
                var paginationMetadata = PaginationMetadata<VehicleResponseDto>.CreatePaginationMetadata(result.Data);
                Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));
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
            var result = await _vehicleService.GetVehicleByIdAsync(id);
            if (result.Success)
            {
                return StatusCode((int)result.StatusCode, result.Data);
            }
            else
            {
                return StatusCode((int)result.StatusCode, result.Message);
            }
        }

        [HttpGet("{licensePlate}")]
        public async Task<IActionResult> GetByLicensePlate(string licensePlate)
        {
            if (!Regex.IsMatch(licensePlate, "^[a-zA-Z0-9]+$"))
            {
                return BadRequest("Placa invalida, a placa deve conter apenas letras e numeros!");
            }
            var result = await _vehicleService.GetVehicleByLicensePlateAsync(licensePlate);
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
        public async Task<IActionResult> Create(VehicleRequestDto vehicleCreateDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(vehicleCreateDto.CustomerId <= 0)
            {
                return BadRequest("ID do veiculo invalido.");
            }
            var result = await _vehicleService.CreateVehicle(vehicleCreateDto);
            if (result.Success)
            {
                return CreatedAtAction(nameof(GetById), new { id = result.Data.VehicleId }, result.Data);
            }
            return StatusCode((int)result.StatusCode, result.Message);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, VehicleRequestDto vehicleUpdateDto)
        {
            if (id <= 0)
            {
                return BadRequest("ID do veiculo invalido.");
            }
            vehicleUpdateDto.VehicleId = id;
            var result = await _vehicleService.UpdateVehicleAsync(vehicleUpdateDto);
            return StatusCode((int)result.StatusCode, result.Message);
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest("ID do veiculo invalido.");
            }
            var result = await _vehicleService.DeleteVehicleAsync(id);
            return StatusCode((int)result.StatusCode, result.Message);
        }
    }
}
