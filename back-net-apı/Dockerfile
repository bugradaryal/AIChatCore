FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY back-net-api.sln .
COPY API/API.csproj API/
COPY Business/Business.csproj Business/
COPY DataAccess/DataAccess.csproj DataAccess/
COPY DTO/DTO.csproj DTO/
COPY Entities/Entities.csproj Entities/
COPY Utilities/Utilities.csproj Utilities/
RUN dotnet restore
COPY . .
WORKDIR /src/API
RUN dotnet publish -c Release -o /app/publish
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "API.dll"]