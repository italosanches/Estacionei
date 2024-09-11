using Estacionei.Context;
using Estacionei.Models;
using Estacionei.Repository.Interfaces;

namespace Estacionei.Repository
{
    public class ConfiguracaoValorHoraRepository : Repository<ConfiguracaoValorHora> , IConfiguracaoValorHoraRepository
    {
        public ConfiguracaoValorHoraRepository(AppDbContext context) : base(context)
        {

        }
    }
}
