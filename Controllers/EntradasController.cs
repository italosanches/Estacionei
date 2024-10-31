using Estacionei.DTOs.Entrada;
using Estacionei.Pagination;
using Estacionei.Pagination.Parameters;
using Estacionei.Pagination.Parameters.EntradaParameters;
using Estacionei.Response;
using Estacionei.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace Estacionei.Controllers
{
    [Route("api/entradas")]
    [ApiController]
    //[Authorize(Policy = "UserOnly")]
    public class EntradasController : ControllerBase
    {
        private readonly IEntradaService _entradaService;

        public EntradasController(IEntradaService entradaService)
        {
            _entradaService = entradaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPaginated([FromQuery] EntradaQueryParameters queryParameters)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _entradaService.GetAllEntradas(queryParameters);
            if (result.Success)
            {
                var paginationMetadata = PaginationMetadata<EntradaResponseDto>.CreatePaginationMetadata(result.Data);
                Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));
                return StatusCode((int)result.StatusCode, result.Data);
            }
            return StatusCode((int)result.StatusCode, result.Message);

           

        }
        [HttpPost]
        public async Task<IActionResult> Create(EntradaRequestDto entradaRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _entradaService.CreateEntrada(entradaRequestDto);
            if (result.Success)
            {
                return CreatedAtAction(nameof(GetById), new { id = result.Data.EntradaId }, result.Data);
            }
            return StatusCode((int)result.StatusCode, result.Message);

        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id <= 0)
            {
                return BadRequest("Id invalido.");
            }
            var result = await _entradaService.GetEntradaById(id);
            if (result.Success)
            {
                return StatusCode((int)result.StatusCode, result.Data);
            }
            return StatusCode((int)result.StatusCode, result.Message);

        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, EntradaRequestDto entradaRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(id <=0 )
            {
                return BadRequest("Id invalido.");
            }
            entradaRequestDto.EntradaId = id;
            var result = await _entradaService.Update(entradaRequestDto);
            return StatusCode((int)result.StatusCode, result.Message);
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id <= 0)
            {
                return BadRequest("Id invalido.");
            }
            var result = await _entradaService.Delete(id);
            return StatusCode((int)result.StatusCode, result.Message);
        }

        
    }
}
