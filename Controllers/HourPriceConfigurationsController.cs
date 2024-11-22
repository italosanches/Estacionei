using AutoMapper;
using Estacionei.DTOs.ConfiguracaoValorHora;
using Estacionei.Enums;
using Estacionei.Repository.Interfaces;
using Estacionei.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Estacionei.Controllers
{
    [Route("api/hourPriceConfiguration")]
    [ApiController]
    [Authorize(Policy = "UserOnly")]
    public class HourPriceConfigurationsController : ControllerBase
    {
        private readonly IHourPriceConfiguration _hourPriceConfService;

        public HourPriceConfigurationsController(IHourPriceConfiguration hourPriceConfService)
        {
            _hourPriceConfService = hourPriceConfService;
            
        }

        [HttpGet]

        public async Task<IActionResult> GetAll()
        {
            var result = await _hourPriceConfService.GetAllConf();
            return StatusCode((int)result.StatusCode, result.Data);
        }

        [HttpGet("{id:int}")]

        public async Task<IActionResult> GetById(int id)
        {
            var result = await _hourPriceConfService.GetById(id);
            if (result.Success)
            {
                return StatusCode((int)result.StatusCode, result.Data);
            }
            return StatusCode((int)result.StatusCode, result.Message);

        }
        [HttpGet("vehicleType/{type:int}")]
        public async Task<IActionResult> GetByTipo(int type)
        {
            var result = await _hourPriceConfService.GetByVehicleType((VehicleType)type);
            if (result.Success)
            {
                return StatusCode((int)result.StatusCode, result.Data);
            }
            return StatusCode((int)result.StatusCode, result.Message);

        }


        [HttpPost]
        public async Task<IActionResult> Create(HourPriceConfigurationRequestDto hourPriceConfRequestDto)
        {
            var result = await _hourPriceConfService.CreateConf(hourPriceConfRequestDto);
            if (result.Success)
            {
                return CreatedAtAction(nameof(GetById), new { id = result.Data.HourPriceConfigurationId }, result.Data);
            }
            return StatusCode((int)result.StatusCode,result.Message);

        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, HourPriceConfigurationRequestDto hourPriceConfRequestDto)
        {
            hourPriceConfRequestDto.HourPriceConfigurationId = id;
            var result = await _hourPriceConfService.UpdateConf(hourPriceConfRequestDto);
            return StatusCode((int)result.StatusCode, result.Message);
        }
    }
}
