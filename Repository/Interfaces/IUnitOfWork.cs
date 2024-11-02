namespace Estacionei.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        IClienteRepository ClienteRepository { get; }
        IVeiculoRepository VeiculoRepository { get; }

        IEntradaRepository EntradaRepository { get; }
        ISaidaRepository SaidaRepository { get; }

        IConfiguracaoValorHoraRepository ConfiguracaoValorHoraRepository { get; }


        Task Commit();
        Task Dispose();
    }
}
