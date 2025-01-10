FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Api/Studweb.Api/Studweb.Api.csproj", "Api/Studweb.Api/"]
COPY ["Core/Studweb.Application/Studweb.Application.csproj", "Core/Studweb.Application/"]
COPY ["Core/Studweb.Domain/Studweb.Domain.csproj", "Core/Studweb.Domain/"]
COPY ["External/Studweb.Infrastructure/Studweb.Infrastructure.csproj", "External/Studweb.Infrastructure/"]
RUN dotnet restore "Api/Studweb.Api/Studweb.Api.csproj"
COPY . .
WORKDIR "/src/Api/Studweb.Api"
RUN dotnet build "Studweb.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Studweb.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Studweb.Api.dll"]