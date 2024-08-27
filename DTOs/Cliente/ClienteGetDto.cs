using System.Text.Json.Serialization;

namespace Estacionei.DTOs.Cliente
{
    public class ClienteGetDto
    {
        public int ClienteId { get; set; }

        public string ClienteNome { get; set; }


        public string ClienteTelefone { get; set; }
    }
}
