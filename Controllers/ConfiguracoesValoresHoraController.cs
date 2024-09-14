using AutoMapper;
using Estacionei.DTOs.ConfiguracaoValorHora;
using Estacionei.Enums;
using Estacionei.Repository.Interfaces;
using Estacionei.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Estacionei.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfiguracoesValoresHoraController : ControllerBase
    {
        private readonly IConfiguracaoValorHoraService _confValoHoraService;
        private readonly IMapper _mapper;

        public ConfiguracoesValoresHoraController(IConfiguracaoValorHoraService confValoHoraService, IMapper mapper)
        {
            _confValoHoraService = confValoHoraService;
            _mapper = mapper;
        }

        [HttpGet]

        public async Task<IActionResult> GetAll()
        {
            var result = await _confValoHoraService.GetAllConf();
            return StatusCode((int)result.StatusCode, result.Data);
        }

        [HttpGet("{id:int}")]

        public async Task<IActionResult> GetById(int id)
        {
            var result = await _confValoHoraService.GetById(id);
            if (result.Success)
            {
                return StatusCode((int)result.StatusCode, result.Data);
            }
            return StatusCode((int)result.StatusCode, result.Message);

        }
        [HttpGet("TipoVeiculo/{tipo:int}")]
        public async Task<IActionResult> GetByTipo(int tipo)
        {
            var result = await _confValoHoraService.GetByTipoVeiculo((TipoVeiculo)tipo);
            if (result.Success)
            {
                return StatusCode((int)result.StatusCode, result.Data);
            }
            return StatusCode((int)result.StatusCode, result.Message);

        }


        [HttpPost]
        public async Task<IActionResult> Create(ConfiguracaoValorHoraCreateDto configuracaoValorHoraCreateDto)
        {
            var result = await _confValoHoraService.CreateConf(configuracaoValorHoraCreateDto);
            if (result.Success)
            {
                return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result.Data);
            }
            return StatusCode((int)result.StatusCode,result.Message);

        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, ConfiguracaoValorHoraUpdateDto configuracaoValorHoraUpdateDto)
        {
            configuracaoValorHoraUpdateDto.Id = id;
            var result = await _confValoHoraService.UpdateConf(configuracaoValorHoraUpdateDto);
            return StatusCode((int)result.StatusCode, result.Message);
        }
    }
}
