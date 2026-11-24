# CEPinho - Etapa 01 e 02: Estruturação e Arquitetura da Solução (.NET 10)

Este documento registra a criação inicial da solução **CEPinho** utilizando apenas a CLI do .NET (sem templates prontos) e define a arquitetura de separação de responsabilidades entre Backend (`CEPinho.Core`) e Frontend (`CEPinho.Web`).

---

## 1. Criação da Estrutura, Solution e Projetos (Step-01)

Abra o terminal (PowerShell ou Bash), navegue até o diretório desejado e execute os comandos abaixo em sequência:

```bash
# 1. Cria a pasta raiz do projeto e entra nela
mkdir CEPinho
cd CEPinho

# 2. Cria o arquivo de Solution (.sln)
dotnet new sln -n CEPinho

# 3. Cria a pasta 'src' para organizar todo o código-fonte
mkdir src
cd src

# 4. Cria o projeto de Backend (Biblioteca de Classes pura, sem boilerplate web)
dotnet new classlib -n CEPinho.Core -f net10.0

# 5. Cria o projeto Web vazio (sem Razor Pages pré-configurado, sem Bootstrap, sem pastas extras)
dotnet new web -n CEPinho.Web -f net10.0

# 6. Volta para a raiz e registra ambos os projetos na Solution
cd ..
dotnet sln add src/CEPinho.Core/CEPinho.Core.csproj
dotnet sln add src/CEPinho.Web/CEPinho.Web.csproj

# 7. Cria a referência: o projeto Web (Front) precisa enxergar o Core (Back)
dotnet add src/CEPinho.Web/CEPinho.Web.csproj reference src/CEPinho.Core/CEPinho.Core.csproj
```

### Limpeza de arquivos gerados automaticamente
Para manter o controle total de cada arquivo criado na aplicação, remova a classe padrão gerada pelo comando `classlib`:

```bash
rm src/CEPinho.Core/Class1.cs
```

### Validação da Solution
Para confirmar que ambos os projetos foram vinculados corretamente à solution, execute na raiz (`CEPinho/`):

```bash
dotnet sln list
```

**Saída esperada:**
```text
src\CEPinho.Core\CEPinho.Core.csproj
src\CEPinho.Web\CEPinho.Web.csproj
```

---

## 2. Por que a pasta `src` fica na raiz da Solution?

No padrão profissional do ecossistema .NET, a pasta `src` (*source* / código-fonte) fica na **raiz do repositório** e agrupa todos os projetos da aplicação (`CEPinho.Core`, `CEPinho.Web`, etc.), e não dentro de cada biblioteca individualmente.

### Vantagens dessa organização:
* **Separação clara:** Mantém o código da aplicação isolado de arquivos de configuração e documentação na raiz (`.gitignore`, `README.md`, `CEPinho.sln`, `Dockerfile`).
* **Escalabilidade:** Facilita a adição futura de uma pasta `tests/` na raiz para testes unitários (ex: `tests/CEPinho.Core.Tests`), mantendo a mesma hierarquia limpa.

```text
CEPinho/                        <-- Raiz do repositório
├── .gitignore                  <-- Configurações do Git (futuro)
├── README.md                   <-- Documentação do projeto
├── CEPinho.sln                 <-- Arquivo de Solution
├── src/                        <-- TODO o código-fonte da aplicação
│   ├── CEPinho.Core/           <-- Projeto 1 (Backend / Regras e API)
│   │   └── CEPinho.Core.csproj
│   └── CEPinho.Web/            <-- Projeto 2 (Frontend / Razor Pages)
│       └── CEPinho.Web.csproj
└── tests/                      <-- Projetos de teste (futuro)
```

---

## 3. Mapa de Responsabilidades e Pastas (Step-02)

A arquitetura abaixo prepara o projeto para a consulta atual via API (ViaCEP) e já deixa o terreno pronto para a futura persistência em **SQLite** sem necessidade de reescrever a interface web:

```text
CEPinho/
├── CEPinho.sln
└── src/
    ├── CEPinho.Core/                  <-- Camada de Backend / Regras e Integrações
    │   ├── Models/
    │   │   └── Endereco.cs            (Representa os dados do endereço no domínio)
    │   ├── DTOs/
    │   │   └── ViaCepResponse.cs      (Mapeia exatamente o JSON retornado pela API ViaCEP)
    │   ├── Interfaces/
    │   │   └── ICepService.cs         (Contrato do serviço de busca de CEP)
    │   └── Services/
    │       └── ViaCepService.cs       (Implementação que realiza o GET HTTP na API)
    │
    └── CEPinho.Web/                   <-- Camada de Apresentação (Razor Pages)
        ├── Pages/
        │   ├── Shared/
        │   │   └── _Layout.cshtml     (Estrutura HTML base, onde entrará o Bootstrap)
        │   ├── _ViewImports.cshtml    (Importa TagHelpers e namespaces para as páginas)
        │   ├── _ViewStart.cshtml      (Define o _Layout padrão para todas as páginas)
        │   ├── Index.cshtml           (HTML da tela de busca de CEP)
        │   └── Index.cshtml.cs        (PageModel: código C# que controla a tela)
        ├── wwwroot/                   (Arquivos estáticos: CSS, JS, logo e favicon)
        └── Program.cs                 (Configuração do servidor web e Injeção de Dependência)
```

### Divisão de Responsabilidades:
1. **`CEPinho.Core`:** Não possui conhecimento sobre HTML, navegadores ou Razor Pages. Recebe um CEP, consulta a fonte externa e retorna um objeto de domínio (`Endereco`).
2. **`CEPinho.Web`:** Não conhece detalhes de HTTP ou desserialização JSON da API ViaCEP. Apenas captura a entrada do usuário, aciona a interface `ICepService` e renderiza o resultado na tela.