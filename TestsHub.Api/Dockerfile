FROM mcr.microsoft.com/dotnet/core/aspnet:3.0.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0.101-buster AS build
WORKDIR /src
COPY ["TestsHub.Api/TestsHub.Api.csproj", "TestsHub.Api/"]
RUN dotnet restore "TestsHub.Api/TestsHub.Api.csproj"
COPY . .
WORKDIR "/src/TestsHub.Api"
RUN dotnet build "TestsHub.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "TestsHub.Api.csproj" -c Release -o /app

ENV ASPNETCORE_ENVIRONMENT Docker

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TestsHub.Api.dll"]