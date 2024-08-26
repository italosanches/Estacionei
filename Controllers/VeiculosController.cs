using Estacionei.DTOs;
using Estacionei.Models;
using Estacionei.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost]
        public async Task<IActionResult> Create(VeiculoDto veiculoDto) 
        {
            try
            {
               var result = await _veiculoService.AddVeiculoAsync(veiculoDto);
                if (!result.Success) 
                {
                    return BadRequest(result.Message);
                }
              return Ok(result);
                
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
