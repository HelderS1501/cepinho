namespace CEPinho.Core.Models
{
    public class Endereco
    {
        public string CEP { get; set; } = string.Empty;
        public string Logradouro { get; set; } = string.Empty;
        public string Complemento { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string UF { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string DDD { get; set; } = string.Empty;
    }
}