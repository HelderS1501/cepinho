using System.Net.Http.Json;
using CEPinho.Core.DTOs;
using CEPinho.Core.Interfaces;
using CEPinho.Core.Models;

namespace CEPinho.Core.Services;

public class ViaCepService : ICepService
{
    private readonly HttpClient _httpClient;

    public ViaCepService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Endereco?> BuscarPorCepAsync(string cep, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(cep))
            return null;

        // Remove hífen, pontos ou espaços, mantendo apenas os números (ex: "01001-000" -> "01001000")
        var cepLimpo = new string(cep.Where(char.IsDigit).ToArray());

        // Todo CEP brasileiro válido possui exatamente 8 dígitos
        if (cepLimpo.Length != 8)
            return null;

        var response = await _httpClient.GetFromJsonAsync<ViaCepResponse>(
        $"https://viacep.com.br/ws/{cepLimpo}/json/",
        cancellationToken);

        // O ViaCEP retorna HTTP 200 com { "erro": true } quando o formato tem 8 dígitos, mas o CEP não existe
        if (response is null || response.Erro)
            return null;

        // Mapeia o DTO externo para o nosso Model limpo de domínio
        return new Endereco
               {
               CEP = response.Cep,
               Logradouro = response.Logradouro,
               Complemento = response.Complemento,
               Bairro = response.Bairro,
               Cidade = response.Localidade,
               UF = response.Uf,
               Estado = response.Estado,
               DDD = response.Cidade
               };
    }
}