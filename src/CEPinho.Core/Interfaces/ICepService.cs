using CEPinho.Core.Models;

namespace CEPinho.Core.Interfaces
{
    public interface ICepService
    {
        Task<Endereco?> BuscarPorCepAsync(string cep, CancellationToken cancellationToken = default);
    }
}

/* 
 Task<Endereco?>: Como a consulta passa pela rede (I/O), o método é assíncrono (Task) 
 para não travar a thread do servidor enquanto aguarda a resposta do ViaCEP. 
 O ponto de interrogação (Endereco?) indica explicitamente que o retorno 
 pode ser null caso o CEP não seja encontrado ou seja inválido.
 
 CancellationToken: Boa prática em .NET moderno; se o usuário fechar a aba do navegador 
 no meio da requisição, o .NET cancela a chamada HTTP automaticamente para poupar recursos do container.
*/