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
    [Route("api/entries")]
    [ApiController]
    [Authorize(Policy = "Useronly")]
    public class EntriesController : ControllerBase
    {
        private readonly IEntryService _entryService;

        public EntriesController(IEntryService entryService)
        {
            _entryService = entryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPaginated([FromQuery] EntryQueryParameters entryQueryParameters)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _entryService.GetAllEntries(entryQueryParameters);
            if (result.Success)
            {
                var paginationMetadata = PaginationMetadata<EntryResponseDto>.CreatePaginationMetadata(result.Data);
                Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));
                return StatusCode((int)result.StatusCode, result.Data);
            }
            return StatusCode((int)result.StatusCode, result.Message);

           

        }
        [HttpPost]
        public async Task<IActionResult> Create(EntryRequestDto entryRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _entryService.CreateEntry(entryRequestDto);
            if (result.Success)
            {
                return CreatedAtAction(nameof(GetById), new { id = result.Data.EntryId }, result.Data);
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
            var result = await _entryService.GetEntryById(id);
            if (result.Success)
            {
                return StatusCode((int)result.StatusCode, result.Data);
            }
            return StatusCode((int)result.StatusCode, result.Message);

        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, EntryRequestDto entryRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(id <=0 )
            {
                return BadRequest("Id invalido.");
            }
            entryRequestDto.EntryId = id;
            var result = await _entryService.UpdateAsync(entryRequestDto);
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
            var result = await _entryService.DeleteAsync(id);
            return StatusCode((int)result.StatusCode, result.Message);
        }

        
    }
}
