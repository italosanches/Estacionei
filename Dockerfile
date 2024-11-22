# Etapa base para executar o serviço no contêiner de produção
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
ENV ASPNETCORE_ENVIRONMENT=Development 


# Etapa de construção do projeto e restauração das dependências
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Estacionei.csproj", "."]
RUN dotnet restore "./Estacionei.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./Estacionei.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Etapa de publicação do projeto
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Estacionei.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

## Etapa para rodar as migrations
#FROM publish AS migrations
#RUN dotnet tool install --global dotnet-ef --version 8.0.0
#ENV PATH="$PATH:/root/.dotnet/tools"
#WORKDIR /src  # Defina o diretório de trabalho onde o projeto está localizado
#ENTRYPOINT ["dotnet-ef", "database", "update", "--project", "Migrations/Estacionei.csproj", "--startup-project", "Estacionei.csproj"]


# Etapa final para execução do projeto no contêiner de produção
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Estacionei.dll"]
