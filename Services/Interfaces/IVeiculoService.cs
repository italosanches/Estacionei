﻿using Estacionei.DTOs;
using Estacionei.DTOs.Veiculo;
using Estacionei.DTOs.Veiculos;
using Estacionei.Models;
using Estacionei.Response;

namespace Estacionei.Services.Interfaces
{
    public interface IVeiculoService
    {
        Task<ResponseBase<IEnumerable<VeiculoGetDto>>> GetAllVeiculoAsync();
        Task<ResponseBase<IEnumerable<VeiculoGetDto>>> GetAllVeiculoByClienteAsync(int clienteId);

        Task<ResponseBase<VeiculoGetDto>> GetVeiculoByIdAsync(int id);
        Task<ResponseBase<VeiculoGetDto>> GetVeiculoByPlacaAsync(string placa);
        

        Task<bool> CheckPlate(string placa);
        Task<bool> CheckPlate(Veiculo veiculo);
        Task<ResponseBase<VeiculoGetDto>> AddVeiculoAsync(VeiculoRequestCreateDto veiculoCreateDto);
        //Task<ResponseBase<VeiculoGetDto>> AddClienteVeiculoAsync(VeiculoRequestCreateDto veiculoCreateDto);

        Task<ResponseBase<bool>> UpdateVeiculoAsync(VeiculoRequestUpdateDto veiculoUpdateDto);
        Task<ResponseBase<bool>> DeleteVeiculoAsync(int id);
    }
}
