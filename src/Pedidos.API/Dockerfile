# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia a solução e os projetos
COPY PedidosStefanini.sln ./
COPY src/Pedidos.API/Pedidos.API.csproj src/Pedidos.API/
COPY src/Pedidos.Application/Pedidos.Application.csproj src/Pedidos.Application/
COPY src/Pedidos.Domain/Pedidos.Domain.csproj src/Pedidos.Domain/
COPY src/Pedidos.Infrastructure/Pedidos.Infrastructure.csproj src/Pedidos.Infrastructure/

# Restaura dependências
RUN dotnet restore PedidosStefanini.sln

# Copia o restante do código
COPY src/ ./src/

# Publica em Release
RUN dotnet publish src/Pedidos.API/Pedidos.API.csproj -c Release -o /app/publish

# Etapa final (runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080

# Variáveis de ambiente
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
ENV DOTNET_RUNNING_IN_CONTAINER=true

# Copia build publicado
COPY --from=build /app/publish .

# Entrypoint
ENTRYPOINT ["dotnet", "Pedidos.API.dll"]