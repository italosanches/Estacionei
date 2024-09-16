namespace Estacionei.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        IClienteRepository ClienteRepository { get; }
        IVeiculoRepository VeiculoRepository { get; }

        IEntradaRepository EntradaRepository { get; }

        IConfiguracaoValorHoraRepository ConfiguracaoValorHoraRepository { get; }

        Task Commit();
        Task Dispose();
    }
}
