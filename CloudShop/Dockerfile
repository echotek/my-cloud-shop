# Этап 1: Берём образ с компилятором .NET 10.0 для сборки кода
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Копируем файл проекта и скачиваем библиотеки
COPY ["CloudShop.csproj", "./"]
RUN dotnet restore

# Копируем весь код и компилируем проект
COPY . .
RUN dotnet publish -c Release -o /app/publish

# Этап 2: Берём лёгкий образ среды выполнения .NET 10.0
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .

# Приложение слушает порт 80 (стандарт для облачных платформ)
EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80

ENTRYPOINT ["dotnet", "CloudShop.dll"]
