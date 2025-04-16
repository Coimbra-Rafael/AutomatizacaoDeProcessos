using EmissaoAutomatica.WebScraping.Model;
using EmissaoAutomatica.WebScrapingFramework.Model;
using EmissaoAutomaticaNFSE.WebScrapingCore.Persistence;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace EmissaoAutomaticaNFSE.WebScrapingCore;

public partial class EmissaoNFSe : Form
{
    private readonly string? _connectionString;
    public EmissaoNFSe(string connectionString)
    {
        InitializeComponent();
        _connectionString = connectionString;
    }
    private async void EmissaoNFSE_Load(object sender, EventArgs e)
    {
        try
        {
            while (true)
            {
                dataGridView1.DataSource = await new NotaFiscalDeServicoDao(_connectionString).BuscarNotasPendentesAsync();
                if (dataGridView1.RowCount > 0)
                    await Task.Run(async () => await RunContinuousProcess());
                dataGridView1.DataSource = await new NotaFiscalDeServicoDao(_connectionString).BuscarNotasTransferidasAsync();
                await Task.Delay(Convert.ToInt32(tempoText.Text));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    #region Métodos

    private async Task RunContinuousProcess()
    {
        try
        {
            using (var notaFiscalDeServicoDao = new NotaFiscalDeServicoDao(_connectionString))
            {
                var notasNaoGeradas = await notaFiscalDeServicoDao.BuscarNotasPendentesAsync();

                var chromeOptions = new ChromeOptions();
                chromeOptions.AddArgument("--start-maximized");
                if (!ativaDesativaNavegadorCheckBox.Checked)
                {
                    chromeOptions.AddArgument("--headless");
                }

                chromeOptions.AddArgument("--disable-gpu");
                chromeOptions.AddArgument("--disable-notifications");
                chromeOptions.AddArgument("--disable-logging");
                chromeOptions.AddArgument("--silent");
                chromeOptions.EnableDownloads = true;
                chromeOptions.AddUserProfilePreference("download.default_directory", "C:\\Notas\\Temp\\");
                chromeOptions.AddUserProfilePreference("download.prompt_for_download", false);
                chromeOptions.AddUserProfilePreference("plugins.always_open_pdf_externally", false);
                chromeOptions.AddUserProfilePreference("profile.default_content_setting_values.automatic_downloads", 1);


                using (var chromeDriver = new ChromeDriver(chromeOptions))
                {
                    var notaFiscalDeServicos = notasNaoGeradas as NotaFiscalDeServico[] ?? notasNaoGeradas.ToArray();
                    if (notaFiscalDeServicos.Count() > 0)
                    {
                        foreach (NotaFiscalDeServico notaNaoGerada in notaFiscalDeServicos)
                        {
                            if (!string.IsNullOrEmpty(notaNaoGerada.ParametroNFSE))
                            {
                                var (pdfBase64, erros) =
                                    await EmitirNota(chromeDriver, notaNaoGerada.ParametroNFSE.Split('|'));
                                await notaFiscalDeServicoDao.AtualizaSituacaoDasNotasAsync(File.ReadAllBytes(pdfBase64),
                                    notaNaoGerada.ParametroNFSE, pdfBase64, erros);
                            }
                        }
                    }

                    chromeDriver.Close();
                    chromeDriver.Quit();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private async Task<Tuple<string, string>> EmitirNota(IWebDriver element, string[] parametros)
    {
        string? erros = null;
        string? pdfBase64 = null;
        try
        {
            var cliente = new Clientes(parametros);

            if (element.Url.Equals("data:,"))
            {
                await element.Navigate().GoToUrlAsync("https://www.nfe-cidades.com.br/landing-page");

                element.FindElement(By.Name("iss_username")).SendKeys(""); //Adicionar o CNPJ para aceesar o portal
                element.FindElement(By.Name("iss_password")).SendKeys(""); //Adicionar a senha para acessar o portal
                element.FindElement(
                    By.XPath(
                        "/html/body/app-root/landing-page/div[2]/div[1]/dx-form/div/div/div/div/div/div/div/div/div/div/div/div/div/div/div[2]/div/div/div/div/form/div/button"
                    )).Submit();

                await Task.Delay(5000);
                await element.Navigate().GoToUrlAsync("https://www.nfe-cidades.com.br/app/emissaonf2.action");
            }
            else if (!element.Url.Equals("https://www.nfe-cidades.com.br/app/emissaonf2.action"))
            {
                await element.Navigate().GoToUrlAsync("https://www.nfe-cidades.com.br/app/emissaonf2.action");
                await Task.Delay(7000);
            }


            element.FindElement(By.Id("emissaonf2_nf_dadosTomador_documento")).SendKeys(cliente.CnpjCpf);

            var action = new Actions(element);
            action.KeyDown(OpenQA.Selenium.Keys.Tab).SendKeys("t").KeyUp(OpenQA.Selenium.Keys.Tab).Build().Perform();

            await Task.Delay(3000);

            if (string.IsNullOrEmpty(
                    element.FindElement(By.Id("emissaonf2_nf_dadosTomador_nome")).GetAttribute("value")))
            {
                element.FindElement(By.Id("emissaonf2_salvarTomador")).Click();
                element.FindElement(By.Id("emissaonf2_nf_dadosTomador_nome")).SendKeys(cliente.NomeCliente);
                element.FindElement(By.Id("emissaonf2_nf_enderecoTomador_cepFmt")).SendKeys(cliente.Cep);

                element.FindElement(By.Id("emissaonf2_nf_enderecoTomador_endereco")).SendKeys(cliente.Logradouro);
                element.FindElement(By.Id("emissaonf2_nf_enderecoTomador_numero")).SendKeys(cliente.NumeroEndereco);
                element.FindElement(By.Id("emissaonf2_nf_enderecoTomador_bairro")).SendKeys(cliente.Bairro);
                await Task.Delay(3500);
                var response = await TrazEnderecoViaCep(cliente.Cep);
                var posicaoInicial = response.IndexOf("<localidade>", StringComparison.Ordinal);
                var posicaoFinal = response.IndexOf("</localidade>", StringComparison.Ordinal);

                if (posicaoInicial >= 0 || posicaoFinal >= 0)
                {
                    int inicio = posicaoInicial + 12;
                    int tamanho = posicaoFinal - inicio;

                    response = response.Substring(inicio, tamanho);
                }

                new SelectElement(element.FindElement(By.Id("estadoEndereco"))).SelectByText(cliente.Estado);
                new SelectElement(element.FindElement(By.Id("municipioEndereco"))).SelectByText(
                    response.Contains("erro") ? "Várzea Paulista" : response);
            }

            element.FindElement(By.Id("emissaonf2_nf_itens_0__descricao")).SendKeys(cliente.DescricaoNotaFiscal);

            element.FindElement(By.Id("emissaonf2_nf_itens_0__valor")).SendKeys(cliente.Valor);
            await Task.Delay(5000);
            element.FindElement(By.Id("btnEmitir")).Click();
            await Task.Delay(10000);

            var directory = new DirectoryInfo(@"C:\Notas\Temp\");

            if (!directory.Exists)
                Directory.CreateDirectory(directory.FullName);

            var files = directory.GetFiles();

            var finalFilePath = @"C:\Notas\" + files[0].Name;
            File.Copy(files[0].FullName, finalFilePath);
            if (File.Exists(files[0].FullName))
                File.Delete(files[0].FullName);

            return Tuple.Create(pdfBase64 = finalFilePath, erros)!;
        }
        catch (Exception ex)
        {
            return Tuple.Create(pdfBase64, ex.Message)!;
        }
    }

    private async Task<string> TrazEnderecoViaCep(string cep)
    {
        try
        {
            var response = await new HttpClient().GetAsync($"https://viacep.com.br/ws/{cep}/xml/");

            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    #endregion
}