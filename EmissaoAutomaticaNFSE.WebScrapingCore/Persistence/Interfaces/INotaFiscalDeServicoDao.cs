using EmissaoAutomatica.WebScrapingFramework.Model;

namespace EmissaoAutomaticaNFSE.WebScrapingCore.Persistence.Interfaces
{
    public interface INotaFiscalDeServicoDao : IDisposable
    {
        Task<IEnumerable<NotaFiscalDeServico>> BuscarNotasPendentesAsync();
        Task<IEnumerable<NotaFiscalDeServico>> BuscarNotasTransferidasAsync();
        Task<bool> AtualizaSituacaoDasNotasAsync(byte[] pdfBase64, string parametros, string caminhoNota,string erros);
    }
}
