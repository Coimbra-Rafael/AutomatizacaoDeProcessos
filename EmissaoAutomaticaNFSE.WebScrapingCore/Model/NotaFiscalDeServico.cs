namespace EmissaoAutomatica.WebScrapingFramework.Model
{
    public sealed class NotaFiscalDeServico
    {
        public string ParametroNFSE { get; private set; }
        public string PdfNFSE { get; private set; }
        public string SitRetNFSE { get; private set; }
        public string ErrosNFSE { get; private set; }
        public string CaminhoNotaNFSE { get; set; }

        public NotaFiscalDeServico(string parametroNFSE, string pdfNFSE, string sitRetNFSE, string errosNFSE, string caminhoNotaNFSE)
        {
            ParametroNFSE = parametroNFSE;
            PdfNFSE = pdfNFSE;
            SitRetNFSE = sitRetNFSE;
            ErrosNFSE = errosNFSE;
            CaminhoNotaNFSE = caminhoNotaNFSE;
        }
    }
}
