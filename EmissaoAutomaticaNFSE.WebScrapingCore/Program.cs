using EmissaoAutomaticaNFSE.WebScrapingCore.Persistence;
using EmissaoAutomaticaNFSE.WebScrapingCore.Persistence.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EmissaoAutomaticaNFSE.WebScrapingCore;

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        Application.SetHighDpiMode(HighDpiMode.SystemAware);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        
        Application.Run(new EmissaoNFSe(""));// Adicionar a connection string
    }
}