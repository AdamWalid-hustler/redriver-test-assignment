FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY RedRiverTest.Api/RedRiverTest.Api.csproj RedRiverTest.Api/
RUN dotnet restore RedRiverTest.Api/RedRiverTest.Api.csproj

COPY RedRiverTest.Api/ RedRiverTest.Api/
RUN dotnet publish RedRiverTest.Api/RedRiverTest.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .

# Render sets PORT env var
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "RedRiverTest.Api.dll"]
