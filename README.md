# Emissão Automática de NFSe

Este projeto foi desenvolvido para automatizar a emissão de Notas Fiscais de Serviço (NFSe), integrando a consulta à base de dados com web scraping para acessar o portal e emitir as notas automaticamente.

## Sumário

- [Recursos e Funcionalidades](#recursos-e-funcionalidades)
- [Arquitetura e Tecnologias](#arquitetura-e-tecnologias)
- [Pré-Requisitos](#pré-requisitos)
- [Configuração](#configuração)
- [Como Executar](#como-executar)
- [Fluxo de Execução](#fluxo-de-execução)
- [Personalizações e Considerações](#personalizações-e-considerações)
- [Licença](#licença)

## Recursos e Funcionalidades

- **Consulta e Atualização de Dados:** Realiza a busca de notas fiscais pendentes e transferidas na base de dados.
- **Web Scraping Automatizado:** Utiliza Selenium WebDriver com ChromeDriver para navegar e interagir com o portal de emissão NFSe.
- **Automações de Login e Emissão:** Efetua login no portal e emite as notas com os dados do tomador.
- **Manipulação de Arquivos:** Gerencia o download dos arquivos das notas emitidas, realizando cópia e limpeza dos diretórios temporários.
- **Integração com API Externa:** Consulta dados de endereço via o serviço [ViaCEP](https://viacep.com.br).

## Arquitetura e Tecnologias

- **Plataforma:** Windows Forms
- **Linguagem:** C#
- **Web Scraping:** Selenium WebDriver e ChromeDriver
- **Acesso a Dados:** DAO Pattern para interação com a base de dados
- **Consultas HTTP:** Utilização do HttpClient para comunicação com a API ViaCEP

## Pré-Requisitos

- **IDE:** Visual Studio ou outra IDE compatível com C#
- **.NET:** .NET Framework, .NET Core ou .NET 5+ (conforme a versão utilizada)
- **Pacotes NuGet:** Selenium.WebDriver
- **ChromeDriver:** Certifique-se de que o executável do ChromeDriver esteja no PATH ou corretamente configurado
- **Base de Dados:** Acesso à base configurada para armazenar e atualizar as notas

## Configuração

1. **Connection String**  
   Configure a _connection string_ no construtor da classe `EmissaoNFSe` para que a aplicação se conecte corretamente à base de dados:
   ```csharp
   var emissaoNFSe = new EmissaoNFSe("Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;");
   ```
2 **Parâmetros do Portal**
  No método EmitirNota, preencha os campos de login (CNPJ e senha) para acessar o portal:
   ````csharp
  element.FindElement(By.Name("iss_username")).SendKeys("seuCNPJ");
  element.FindElement(By.Name("iss_password")).SendKeys("suaSenha");
  ````
3. **Configurações do ChromeDriver**
- Modo de Execução: A aplicação permite configurar a execução do ChromeDriver no modo headless ou com o navegador visível, controlado pela checkbox ativaDesativaNavegadorCheckBox.
- Diretório de Download: Os arquivos são baixados para o diretório C:\Notas\Temp\. Verifique as permissões e a existência desse diretório.
4. **Tempo de Espera**
  O delay entre as consultas na base de dados é definido pelo controle tempoText. Ajuste esse valor conforme a necessidade da sua operação.

## Como Executar
1. **Compilação**
   - Abra o projeto no Visual Studio, restaure os pacotes NuGet e compile a solução.
2. **Execução**
  - Execute o aplicativo. Ao iniciar, o sistema buscará as notas fiscais pendentes na base de dados e iniciará o processo contínuo de emissão.

## Fluxo de Execução
1. **Inicialização:**
  Quando o formulário (EmissaoNFSE_Load) é carregado, a aplicação inicia um loop que consulta periodicamente a base de dados para identificar notas pendentes.

2. **Consulta e Processamento:**

- Busca de Dados: Utiliza os métodos BuscarNotasPendentesAsync e BuscarNotasTransferidasAsync para gerenciar os registros.

- Processamento: O método RunContinuousProcess configura o ChromeDriver, realiza o web scraping e, para cada nota pendente, executa:
  - Login e navegação no portal de NFSe.
  - Preenchimento dos dados do tomador, incluindo a consulta via ViaCEP para obtenção do endereço.
  - Emissão da nota e download do arquivo PDF gerado.

3. **Atualização da Base:**
  Após a emissão, o método AtualizaSituacaoDasNotasAsync atualiza o status da nota na base, registrando o arquivo PDF e tratando eventuais erros.

## Personalizações e Considerações

**Tratamento de Erros:**
  - Atualmente, a aplicação captura exceções e imprime mensagens no console. Recomenda-se aprimorar esse processo com logs detalhados e notificações.

**Manutenção dos Seletores:**
 - Caso o layout do portal de emissão mude, os seletores (IDs, XPath) utilizados no web scraping precisarão ser ajustados.

**Segurança:**
  - Evite manter credenciais (como CNPJ e senha) hardcoded no código. Considere utilizar variáveis de ambiente ou arquivos de configuração criptografados.

**Performance e Monitoramento:**
Ajuste o intervalo de tempo das consultas conforme o volume de dados. Implemente um mecanismo de monitoramento para acompanhar o desempenho da aplicação.
