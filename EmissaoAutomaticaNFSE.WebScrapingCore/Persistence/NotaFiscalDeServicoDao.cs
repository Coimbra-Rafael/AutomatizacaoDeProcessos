using System.Data;
using EmissaoAutomatica.WebScrapingFramework.Model;
using EmissaoAutomaticaNFSE.WebScrapingCore.Persistence.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace EmissaoAutomaticaNFSE.WebScrapingCore.Persistence
{
    public sealed class NotaFiscalDeServicoDao : INotaFiscalDeServicoDao
    {
        private readonly string? _connectionString;

        public NotaFiscalDeServicoDao(string? connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task<IEnumerable<NotaFiscalDeServico>> BuscarNotasPendentesAsync()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    List<NotaFiscalDeServico> ListaPendenteNotas = new List<NotaFiscalDeServico>();
                    var query = @"SELECT Parametro_NFSE, PDF_NFSE, SitRet_NFSE, Erros_NFSE, Caminho_PDF_NFSE FROM RectoParcelas WHERE SitRet_NFSE = @Sit ";

                    if (connection.State == ConnectionState.Closed)
                        await connection.OpenAsync();

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Sit", "P");

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var parametro = reader.IsDBNull(reader.GetOrdinal("Parametro_NFSE")) 
                                    ? string.Empty 
                                    : reader.GetString(reader.GetOrdinal("Parametro_NFSE"));

                                var pdf = reader.IsDBNull(reader.GetOrdinal("PDF_NFSE")) 
                                    ? string.Empty 
                                    : reader.GetString(reader.GetOrdinal("PDF_NFSE"));

                                var situacao = reader.IsDBNull(reader.GetOrdinal("SitRet_NFSE")) 
                                    ? string.Empty 
                                    : reader.GetString(reader.GetOrdinal("SitRet_NFSE"));

                                var erros = reader.IsDBNull(reader.GetOrdinal("Erros_NFSE")) 
                                    ? string.Empty 
                                    : reader.GetString(reader.GetOrdinal("Erros_NFSE"));

                                var caminho = reader.IsDBNull(reader.GetOrdinal("Caminho_PDF_NFSE")) 
                                    ? string.Empty 
                                    : reader.GetString(reader.GetOrdinal("Caminho_PDF_NFSE"));

                                var notaFiscal = new NotaFiscalDeServico(parametro, pdf, situacao, erros, caminho);
                                ListaPendenteNotas.Add(notaFiscal);
                            }
                        }
                        connection.Close();
                        return ListaPendenteNotas.AsEnumerable();

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<NotaFiscalDeServico>> BuscarNotasTransferidasAsync()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    List<NotaFiscalDeServico> listaNotasTransferidas = new List<NotaFiscalDeServico>();
                    var query = "SELECT Parametro_NFSE, PDF_NFSE, SitRet_NFSE, Erros_NFSE, Caminho_PDF_NFSE FROM RectoParcelas WHERE SitRet_NFSE = @Sit ";

                    if(connection.State == ConnectionState.Closed)
                        await connection.OpenAsync();
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Sit", "T");

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync()) 
                            {
                                var parametro = reader.IsDBNull(reader.GetOrdinal("Parametro_NFSE"))
                                    ? string.Empty
                                    : reader.GetString(reader.GetOrdinal("Parametro_NFSE"));

                                var pdf = reader.IsDBNull(reader.GetOrdinal("PDF_NFSE"))
                                    ? string.Empty
                                    : reader.GetString(reader.GetOrdinal("PDF_NFSE"));

                                var situacao = reader.IsDBNull(reader.GetOrdinal("SitRet_NFSE"))
                                    ? string.Empty
                                    : reader.GetString(reader.GetOrdinal("SitRet_NFSE"));

                                var erros = reader.IsDBNull(reader.GetOrdinal("Erros_NFSE"))
                                    ? string.Empty
                                    : reader.GetString(reader.GetOrdinal("Erros_NFSE"));

                                var caminho = reader.IsDBNull(reader.GetOrdinal("Caminho_PDF_NFSE"))
                                    ? string.Empty
                                    : reader.GetString(reader.GetOrdinal("Caminho_PDF_NFSE"));

                                var notaFiscal = new NotaFiscalDeServico(parametro, pdf, situacao, erros, caminho);
                                listaNotasTransferidas.Add(notaFiscal);
                            }
                        }
                    }
                    return listaNotasTransferidas;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<bool> AtualizaSituacaoDasNotasAsync(byte[] pdfBase64, string parametros, string caminhoNota, string erros = null)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var query = "UPDATE RectoParcelas " +
                                "SET SitRet_NFSE = @Sit, PDF_NFSE = @Pdf, Caminho_PDF_NFSE = @CaminhoNota, Erros_NFSE = @Erros " +
                                "WHERE Parametro_NFSE = @Parametros AND SitRet_NFSE = @SitP";

                    if (connection.State == ConnectionState.Closed)
                        await connection.OpenAsync();

                    using (var transcation = connection.BeginTransaction())
                    using (SqlCommand command = new SqlCommand(query, connection, transcation))
                    {

                        command.Parameters.AddWithValue("@Parametros", parametros);
                        command.Parameters.AddWithValue("@SitP", "P");
                        if (erros != null)
                        {
                            command.Parameters.AddWithValue("@Sit", "E");
                            command.Parameters.AddWithValue("@Erros", erros);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Sit", "T");
                            command.Parameters.AddWithValue("@Pdf", Convert.ToBase64String(pdfBase64));
                            command.Parameters.AddWithValue("@CaminhoNota", caminhoNota);
                            command.Parameters.AddWithValue("@Erros", erros ?? string.Empty);
                            
                        }

                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        transcation.Commit();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Erro de SQL: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro geral: {ex.Message}");
                throw; 
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
