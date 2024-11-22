using Estacionei.Enums;

namespace Estacionei.Pagination.Parameters.Vehicle
{
    public class VehicleQueryParameters : QueryParameters
    {
        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public VehicleType VehicleType { get; set; }
    }
}
