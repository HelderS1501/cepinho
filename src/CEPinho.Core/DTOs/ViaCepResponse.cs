using System.Text.Json.Serialization;

namespace CEPinho.Core.DTOs
{
    public class ViaCepResponse
    {
        [JsonPropertyName("cep")]
        public string Cep { get; set; } = string.Empty;
        
        [JsonPropertyName("logradouro")]
        public string Logradouro { get; set; } = string.Empty;
        
        [JsonPropertyName("complemento")]
        public string Complemento { get; set; } = string.Empty;
        
        [JsonPropertyName("bairro")]
        public string Bairro { get; set; } = string.Empty;
        
        [JsonPropertyName("localidade")]
        public string Localidade { get; set; } = string.Empty;
        
        [JsonPropertyName("uf")]
        public string Uf { get; set; } = string.Empty;
        
        [JsonPropertyName("estado")]
        public string Estado { get; set; } = string.Empty;
        
        [JsonPropertyName("ddd")]
        public string Cidade { get; set; } = string.Empty;
        
        // Quando o CEP tem 8 dígitos válidos mas não existe na base dos Correios,
        // o ViaCEP retorna HTTP 200 com { "erro": true }
        [JsonPropertyName("erro")]
        public bool Erro { get; set; }
     
    }
}