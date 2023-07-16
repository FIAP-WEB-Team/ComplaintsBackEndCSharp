# Imagem base
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# Diretório de trabalho
WORKDIR /app

# Copiar o arquivo csproj e restaurar dependências
COPY *.csproj ./
RUN dotnet restore

# Copiar o resto do código-fonte e compilar
COPY . ./
RUN dotnet publish -c Release -o out

# Imagem final
FROM mcr.microsoft.com/dotnet/aspnet:7.0

# Diretório de trabalho
WORKDIR /app

# Copiar os arquivos publicados do ambiente de compilação
COPY --from=build /app/out .

# Porta que a aplicação irá expor
EXPOSE 80

# Comando para iniciar a aplicação
ENTRYPOINT ["dotnet", "ComplaintsAPI.dll"]