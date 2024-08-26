namespace Estacionei.Services
{
	public class VeiculoValidationResult
	{
		public bool ValidationResult { get; set; }
        public List<int> IdsNotFound { get; set; } = new List<int>();
		public VeiculoValidationResult(bool validationResult, List<int> idsNotFound)
		{
			ValidationResult = validationResult;
			IdsNotFound = idsNotFound;
		}
	}
}
