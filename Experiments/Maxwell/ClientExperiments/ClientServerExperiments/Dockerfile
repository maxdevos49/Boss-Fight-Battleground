FROM mcr.microsoft.com/dotnet/core/runtime:2.1-stretch-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build
WORKDIR /src
COPY ClientServerExperiments/TestServer.csproj ClientServerExperiments/
RUN dotnet restore "ClientServerExperiments/TestServer.csproj"
COPY . .
WORKDIR "/src/ClientServerExperiments"
RUN dotnet build "TestServer.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "TestServer.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TestServer.dll"]
