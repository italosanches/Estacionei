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
    [Route("api/exits")]
    [ApiController]
    [Authorize(Policy = "UserOnly")]

    public class ExitsController : ControllerBase
    {
        private IExitService _extiService;

        public ExitsController(IExitService extiService)
        {
            _extiService = extiService;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _extiService.GetExitById(id);
            if (result.Success)
            {
                return StatusCode((int)result.StatusCode, result.Data);
            }
            return StatusCode((int)result.StatusCode, result.Message);


        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ExitQueryParameters exitQueryParameters)
        {
            var result = await _extiService.GetAllExits(exitQueryParameters);
            if (result.Success)
            {
                var paginationMetadata = PaginationMetadata<ExitResponseDto>.CreatePaginationMetadata(result.Data);
                Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));
                return StatusCode((int)result.StatusCode, result.Data);
            }
            return StatusCode((int)result.StatusCode, result.Message);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ExitRequestDto exitRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _extiService.CreateExit(exitRequestDto);
            if (result.Success)
            {
                return CreatedAtAction(nameof(GetById), new { id = result.Data.ExitId }, result.Data);
            }
            return StatusCode((int)result.StatusCode, result.Message);
        }

        [HttpPut("{id:int}/exitDate")]

        public async Task<IActionResult> UpdateDateSaida(ExitUpdateRequestDto exitUpdateRequestDto, int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id <= 0)
            {
                return BadRequest("ID invalido.");
            }
            exitUpdateRequestDto.ExitId = id;
            var result = await _extiService.UpdateExitDate(exitUpdateRequestDto);
            if (result.Success)
            {
                return StatusCode((int)result.StatusCode, result.Data);
            }
            return StatusCode((int)result.StatusCode, result.Message);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _extiService.DeleteExit(id);
            return StatusCode((int)result.StatusCode, result.Message);
        }

    }
}
