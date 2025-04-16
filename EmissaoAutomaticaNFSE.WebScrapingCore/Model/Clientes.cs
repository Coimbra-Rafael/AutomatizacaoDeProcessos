namespace EmissaoAutomatica.WebScraping.Model
{
    public sealed class Clientes
    {
        public string NomeCliente { get; private set; }
        public string CnpjCpf { get; private set; }
        public string Cep { get; private set; }
        public string Logradouro { get; private set; }
        public string NumeroEndereco { get; private set; }
        public string Bairro { get; private set; }
        public string Estado { get; private set; }
        public string Municipio { get; private set; }
        public string DescricaoNotaFiscal { get; set; }
        public string Valor { get; private set; }

        public Clientes(string nomeCliente, string cnpjCpf, string cep, string logradouro, string numeroEndereco, string bairro, string estado, string municipio, string descricaoNotaFiscal, string valor)
        {
            NomeCliente = nomeCliente;
            CnpjCpf = cnpjCpf;
            Cep = cep;
            Logradouro = logradouro;
            NumeroEndereco = numeroEndereco;
            Bairro = bairro;
            Estado = estado;
            Municipio = municipio;
            DescricaoNotaFiscal = descricaoNotaFiscal;
            Valor = valor;
        }

        public Clientes(string[] parametros)
        {
            NomeCliente = parametros[0] ?? throw new System.ArgumentNullException("Nome não pode ser vazio!");
            CnpjCpf = parametros[1] ?? throw new System.ArgumentNullException("Cnpj ou Cpf não pode ser vazio!");
            Cep = parametros[2] ?? throw new System.ArgumentNullException("Cep não pode ser vazio!");
            Logradouro = parametros[3] ?? throw new System.ArgumentNullException("Logradouro não pode ser vazio!");
            NumeroEndereco = parametros[4] ?? throw new System.ArgumentNullException("Número não pode ser vazio!");
            Bairro = parametros[5] ?? throw new System.ArgumentNullException("Bairro não pode ser vazio!");
            Estado = parametros[6] ?? throw new System.ArgumentNullException("Estado não pode ser vazio!");
            Municipio = parametros[7] ?? throw new System.ArgumentNullException("Municipio não pode ser vazio!");
            DescricaoNotaFiscal = parametros[8] ?? throw new System.ArgumentNullException("Descrição da nota não pode ser vazio!");
            Valor = parametros[9] ?? throw new System.ArgumentNullException("Valor não pode ser vazio!");
        }
    }
}
