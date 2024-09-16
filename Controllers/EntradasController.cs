using Estacionei.DTOs.Entrada;
using Estacionei.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Estacionei.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntradasController : ControllerBase
    {
        private readonly IEntradaService _entradaService;

        public EntradasController(IEntradaService entradaService)
        {
            _entradaService = entradaService;
        }


        [HttpPost]

        public async Task<IActionResult> Create(EntradaRequestCreateDto entradaRequestCreateDto)
        {
            var result = await _entradaService.CreateEntrada(entradaRequestCreateDto);
            return Ok(result.Data);
        }

    }
}
