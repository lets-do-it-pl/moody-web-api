#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 1234

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/LetsDoIt.Moody.Web/LetsDoIt.Moody.Web.csproj", "src/LetsDoIt.Moody.Web/"]
COPY ["src/LetsDoIt.Moody.Persistence/LetsDoIt.Moody.Persistence.csproj", "src/LetsDoIt.Moody.Persistence/"]
COPY ["src/LetsDoIt.Moody.Infrastructure/LetsDoIt.Moody.Infrastructure.csproj", "src/LetsDoIt.Moody.Infrastructure/"]
COPY ["src/LetsDoIt.Moody.Application/LetsDoIt.Moody.Application.csproj", "src/LetsDoIt.Moody.Application/"]
RUN dotnet restore "src/LetsDoIt.Moody.Web/LetsDoIt.Moody.Web.csproj"
COPY . .
WORKDIR "/src/src/LetsDoIt.Moody.Web"
RUN dotnet build "LetsDoIt.Moody.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LetsDoIt.Moody.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LetsDoIt.Moody.Web.dll"]