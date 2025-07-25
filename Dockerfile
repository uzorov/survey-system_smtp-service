# Используем официальный образ .NET SDK для сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY src/MassMailService ./
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Используем официальный образ .NET Runtime для запуска
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./
COPY src/MassMailService/Templates ./Templates
ENTRYPOINT ["dotnet", "MassMailService.dll"]
