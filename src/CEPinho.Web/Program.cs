using CEPinho.Core.Interfaces;
using CEPinho.Core.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Habilita o suporte a Razor Pages na aplicação
builder.Services.AddRazorPages();

// 2. Registra o ViaCepService com gerenciamento otimizado de HttpClient
builder.Services.AddHttpClient<ICepService, ViaCepService>();

var app = builder.Build();

// 3. Permite servir arquivos estáticos da pasta wwwroot (CSS, JS, imagens, favicon)
app.UseStaticFiles();

// 4. Habilita o roteamento das requisições
app.UseRouting();

// 5. Mapeia as páginas da pasta Pages/ como endpoints acessíveis no navegador
app.MapRazorPages();

app.Run();