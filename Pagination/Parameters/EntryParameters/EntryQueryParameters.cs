namespace Estacionei.Pagination.Parameters.EntradaParameters
{
    public class EntryQueryParameters : QueryParameters
    {
        private DateTime _startDate = DateTime.MinValue;
        private DateTime _endDate = DateTime.MinValue;

        public DateTime StartDate { get { return _startDate; } set { _startDate = value; } }
        public DateTime EndDate { get { return _endDate; } set { _endDate = value; } }

        public int VehicleId { get; set; }
        public string? VehicleLicensePlate { get; set; }


    }
}

