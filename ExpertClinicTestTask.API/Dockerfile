FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ExpertClinicTestTask.API/ExpertClinicTestTask.API.csproj", "ExpertClinicTestTask.API/"]
COPY ["ExpertClinicTestTask.Infrastructure.Data/ExpertClinicTestTask.Infrastructure.Data.csproj", "ExpertClinicTestTask.Infrastructure.Data/"]
COPY ["ExpertClinicTestTask.Domain.Core/ExpertClinicTestTask.Domain.Core.csproj", "ExpertClinicTestTask.Domain.Core/"]
COPY ["ExpertClinicTestTask.Domain.Interfaces/ExpertClinicTestTask.Domain.Interfaces.csproj", "ExpertClinicTestTask.Domain.Interfaces/"]
RUN dotnet restore "ExpertClinicTestTask.API/ExpertClinicTestTask.API.csproj"
COPY . .
WORKDIR "/src/ExpertClinicTestTask.API"
RUN dotnet build "ExpertClinicTestTask.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ExpertClinicTestTask.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExpertClinicTestTask.API.dll"]
