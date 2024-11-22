using AutoMapper;
using Estacionei.DTOs.Cliente;
using Estacionei.DTOs.Customer;
using Estacionei.Enums;
using Estacionei.Models;
using Estacionei.Pagination;
using Estacionei.Pagination.Parameters;
using Estacionei.Pagination.Parameters.ClienteParameters;
using Estacionei.Response;
using Estacionei.Services;
using Estacionei.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace Estacionei.Controllers
{
    [Route("api/customers")]
    [ApiController]
    //[Authorize(Policy = "UserOnly")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] CustomerQueryParameters customerQueryParameters)
        {
            var result = await _customerService.GetAllCustomersByPaginationAsync(customerQueryParameters);

            if (result.Success)
            {
                var paginationMetadata = PaginationMetadata<CustomerResponseDto>.CreatePaginationMetadata(result.Data);
                Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));
                return StatusCode((int)result.StatusCode, result.Data);
            }
            return StatusCode((int)result.StatusCode,result.Message);

        }

        [HttpGet("{id:int}", Name = "GetById")]
        public async Task<IActionResult> GetById(int id)
        {

            var result = await _customerService.GetCustomerByIdAsync(id);
            if (result.Success)
            {
                return StatusCode((int)result.StatusCode, result.Data);
            }
            else
            {
                return StatusCode((int)result.StatusCode, result.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(CustomerRequestCreateDto customerDto)
        {
            if (customerDto.Vehicle is not null)
            {
                bool validateEnum = Enum.IsDefined(typeof(VehicleType), customerDto.Vehicle.VehicleType);
                if (!validateEnum)
                {
                    return BadRequest("Tipo do veiculo é invalido");
                }

            }
            var result = await _customerService.AddCustomerAsync(customerDto);
            if (result.Success)
            {
                return CreatedAtAction(nameof(GetById), new { id = result.Data.CustomerId }, result.Data);
            }
            else
            {
                return StatusCode((int)result.StatusCode, (result.Message));
            }

        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, CustomerRequestUpdateDto customerDto)
        {
            if (id <= 0)
            {
                return BadRequest("ID invalido");
            }
            customerDto.CustomerId = id;
            var result = await _customerService.UpdateCustomerAsync(customerDto);
            return StatusCode((int)result.StatusCode, result.Message);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _customerService.DeleteCustomerAsync(id);
            return StatusCode((int)result.StatusCode, result.Message);
        }

    }
}
