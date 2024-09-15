FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:80;https://+:443
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/App/Taskio/Taskio.csproj", "src/App/Taskio/"]
COPY ["src/Auth/Auth/Auth.csproj", "src/Auth/Auth/"]
COPY ["src/Auth/Auth.Application/Auth.Application.csproj", "src/Auth/Auth.Application/"]
COPY ["src/Shared/AMSaiian.Shared.Application/AMSaiian.Shared.Application.csproj", "src/Shared/AMSaiian.Shared.Application/"]
COPY ["src/Shared/AMSaiian.Shared.Domain/AMSaiian.Shared.Domain.csproj", "src/Shared/AMSaiian.Shared.Domain/"]
COPY ["src/Shared/AMSaiian.Shared.Web/AMSaiian.Shared.Web.csproj", "src/Shared/AMSaiian.Shared.Web/"]
COPY ["src/Auth/Auth.Infrastructure/Auth.Infrastructure.csproj", "src/Auth/Auth.Infrastructure/"]
COPY ["src/Shared/AMSaiian.Shared.Infrastructure/AMSaiian.Shared.Infrastructure.csproj", "src/Shared/AMSaiian.Shared.Infrastructure/"]
COPY ["src/App/Taskio.Application/Taskio.Application.csproj", "src/App/Taskio.Application/"]
COPY ["src/App/Taskio.Domain/Taskio.Domain.csproj", "src/App/Taskio.Domain/"]
COPY ["src/App/Taskio.Infrastructure/Taskio.Infrastructure.csproj", "src/App/Taskio.Infrastructure/"]
RUN dotnet restore "src/App/Taskio/Taskio.csproj"

COPY . .
WORKDIR "/src/src/App/Taskio"
RUN dotnet build "Taskio.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Taskio.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Taskio.dll"]
