using Estacionei.DTOs.Cliente;
using Estacionei.DTOs.Entrada;
using Estacionei.DTOs.Saida;
using Estacionei.Pagination;
using Estacionei.Pagination.Parameters.SaidaParameters;
using Estacionei.Response;
using Estacionei.Services;
using Estacionei.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace Estacionei.Controllers
{
    [Route("api/saidas")]
    [ApiController]
    //[Authorize(Policy = "UserOnly")]

    public class SaidasController : ControllerBase
    {
        private ISaidaService _saidaService;

        public SaidasController(ISaidaService saidaService)
        {
            _saidaService = saidaService;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _saidaService.GetSaida(id);
            if (result.Success)
            {
                return StatusCode((int)result.StatusCode, result.Data);
            }
            return StatusCode((int)result.StatusCode, result.Message);


        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] SaidaQueryParameters saidaQueryParameters)
        {
            var result = await _saidaService.GetAllSaidas(saidaQueryParameters);
            if (result.Success)
            {
                var paginationMetadata = PaginationMetadata<SaidaResponseDto>.CreatePaginationMetadata(result.Data);
                Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));
                return StatusCode((int)result.StatusCode, result.Data);
            }
            return StatusCode((int)result.StatusCode, result.Message);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaidaRequestDto saidaRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _saidaService.CreateSaida(saidaRequestDto);
            if (result.Success)
            {
                return CreatedAtAction(nameof(GetById), new { id = result.Data.SaidaId }, result.Data);
            }
            return StatusCode((int)result.StatusCode, result.Message);
        }

        [HttpPut("{id:int}/dataSaida")]

        public async Task<IActionResult> UpdateDateSaida(SaidaUpdateRequestDto saidaUpdateRequestDto, int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id <= 0)
            {
                return BadRequest("ID invalido.");
            }
            saidaUpdateRequestDto.SaidaId = id;
            var result = await _saidaService.UpdateDateSaida(saidaUpdateRequestDto);
            if (result.Success)
            {
                return StatusCode((int)result.StatusCode, result.Data);
            }
            return StatusCode((int)result.StatusCode, result.Message);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _saidaService.DeleteSaida(id);
            return StatusCode((int)result.StatusCode, result.Message);
        }

    }
}
