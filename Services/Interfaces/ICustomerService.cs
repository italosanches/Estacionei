using Estacionei.DTOs.Cliente;
using Estacionei.Models;
using Estacionei.Pagination.Parameters;
using Estacionei.Pagination;
using Estacionei.Response;
using Estacionei.Pagination.Parameters.ClienteParameters;
using Estacionei.DTOs.Customer;

namespace Estacionei.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<ResponseBase<PagedList<CustomerResponseDto>>> GetAllCustomersByPaginationAsync(CustomerQueryParameters queryParameters);
        Task<ResponseBase<CustomerResponseDto>> GetCustomerByIdAsync(int id);
        Task<ResponseBase<CustomerResponseDto>> AddCustomerAsync(CustomerRequestCreateDto customerDto);
        Task<ResponseBase<bool>> UpdateCustomerAsync(CustomerRequestUpdateDto customer );
        Task<ResponseBase<bool>> DeleteCustomerAsync(int id);
    }
}
