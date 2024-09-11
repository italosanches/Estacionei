using Estacionei.Repository.Interfaces;

namespace Estacionei.Repository
{
    public interface IUnitOfWork
    {
        IClienteRepository ClienteRepository { get; }
        IVeiculoRepository VeiculoRepository { get; }

        IConfiguracaoValorHoraRepository ConfiguracaoValorHoraRepository { get; }

        Task Commit();
        Task Dispose();
    }
}
