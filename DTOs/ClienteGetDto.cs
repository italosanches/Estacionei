using System.Text.Json.Serialization;

namespace Estacionei.DTOs
{
	public class ClienteGetDto
	{
        public int ClientId { get; set; }
		
		public string ClienteNome { get; set; }


		public string ClienteTelefone { get; set; }
	}
}
