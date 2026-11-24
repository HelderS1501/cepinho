# Estágio 1: Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /build

# Copia os arquivos de projeto primeiro para aproveitar o cache de camadas do Docker
COPY ["CEPinho.sln", "./"]
COPY ["src/CEPinho.Core/CEPinho.Core.csproj", "src/CEPinho.Core/"]
COPY ["src/CEPinho.Web/CEPinho.Web.csproj", "src/CEPinho.Web/"]
RUN dotnet restore "CEPinho.sln"

# Copia o restante do código e publica em modo Release
COPY . .
WORKDIR "/build/src/CEPinho.Web"
RUN dotnet publish "CEPinho.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Estágio 2: Runtime (imagem final enxuta e segura)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Cria o diretório do SQLite e entrega a posse ao usuário non-root (UID 1654)
RUN mkdir -p /app/data && chown -R $APP_UID:$APP_UID /app/data

# Executa com usuário não-root padrão da imagem .NET moderna
USER $APP_UID

EXPOSE 8083
ENV ASPNETCORE_URLS=http://+:8083

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CEPinho.Web.dll"]