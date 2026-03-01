FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY TesteTecnico.csproj ./
RUN dotnet restore TesteTecnico.csproj

COPY . .
RUN dotnet publish TesteTecnico.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "TesteTecnico.dll"]