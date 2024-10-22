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
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "UserOnly")]
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
            if (queryParameters.DataInicio > queryParameters.DataFim)
            {
                return BadRequest("Data inicio maior que a data fim.");

            }
            var result = await _entradaService.GetAllEntradas(queryParameters);
            if (!result.Success)
            {
                return StatusCode((int)result.StatusCode, result.Message);
            }
            CreateHeadPagination(result);
            return StatusCode((int)result.StatusCode, result.Data);

        }
        [HttpPost]

        public async Task<IActionResult> Create(EntradaRequestCreateDto entradaRequestCreateDto)
        {
            var result = await _entradaService.CreateEntrada(entradaRequestCreateDto);
            if (!result.Success)
            {
                return StatusCode((int)result.StatusCode, result.Message);
            }
                return StatusCode((int)result.StatusCode, result.Data);
        }

        private void CreateHeadPagination(ResponseBase<PagedList<EntradaResponseDto>> result)
        {
            var metadata = new
            {

                result.Data.Count,
                result.Data.TotalCount,
                result.Data.CurrentPage,
                result.Data.PageSize,
                result.Data.TotalPages,
                result.Data.HasNext,
                result.Data.HasPrevious
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
        }
    }
}
