﻿using Estacionei.DTOs.Cliente;
using Estacionei.Models;
using Estacionei.Response;

namespace Estacionei.Services.Interfaces
{
    public interface IClienteService
    {
        Task<ResponseBase<IEnumerable<ClienteGetDto>>> GetAllClienteAsync();
        Task<ResponseBase<ClienteGetDto>> GetClienteByIdAsync(int id);
        Task<ResponseBase<ClienteGetDto>> AddClienteAsync(ClienteCreateDto clienteDto);
        Task<ResponseBase<ClienteUpdateDto>> UpdateClienteAsync(int id, ClienteUpdateDto clienteDto);
        Task<ResponseBase<Cliente>> DeleteClienteAsync(int id);
    }
}
