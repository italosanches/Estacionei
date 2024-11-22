namespace Estacionei.DTOs.Saida
{
    public class ExitResponseDto
    {
        public int ExitId { get; set; }
        public int EntryId { get; set; }

        public DateTime EntryDate { get; set; }
        public DateTime ExitDate { get; set; }

        public decimal ChargedAmount { get; set; }

        public int VehicleId {  get; set; }
        public string? VehicleLicensePlate { get; set; }
        public string VehicleModel { get; set; }
        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }

    }
}
