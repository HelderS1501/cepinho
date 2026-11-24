using CEPinho.Core.Interfaces;
using CEPinho.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CEPinho.Web.Pages;

public class IndexModel : PageModel
{
    private readonly ICepService _cepService;

    public IndexModel(ICepService cepService)
    {
        _cepService = cepService;
    }

    [BindProperty]
    public string? Cep { get; set; }

    public Endereco? EnderecoEncontrado { get; set; }

    public string? MensagemErro { get; set; }

    public void OnGet()
    {
        // Executado quando o usuário abre a página pela primeira vez (requisição GET)
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(Cep))
        {
            MensagemErro = "Informe um CEP para realizar a consulta.";
            return Page();
        }

        try
        {
            EnderecoEncontrado = await _cepService.BuscarPorCepAsync(Cep, cancellationToken);

            if (EnderecoEncontrado is null)
            {
                MensagemErro = $"Nenhum endereço encontrado para o CEP \"{Cep}\". Verifique os 8 dígitos e tente novamente.";
            }
        }
        catch (HttpRequestException)
        {
            MensagemErro = "Não foi possível conectar ao serviço ViaCEP no momento. Tente novamente em instantes.";
        }

        return Page();
    }
}