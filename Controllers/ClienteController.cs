using Estacionei.Models;
using Estacionei.Response;
using Estacionei.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Estacionei.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpPost]

        public async Task<IActionResult> Create(Cliente cliente)
        { 
           var result = await  _clienteService.AddClienteAsync(cliente);
            if (result.Success) { return Ok(result); }
            else { return BadRequest(); }
            
        }
    }
}
