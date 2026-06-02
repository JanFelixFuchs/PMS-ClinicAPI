FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Utils/Utils.csproj", "Utils/"]
COPY ["ClinicAPI/ClinicAPI.csproj", "ClinicAPI/"]

RUN dotnet restore "ClinicAPI/ClinicAPI.csproj"

COPY . .
RUN dotnet publish "ClinicAPI/ClinicAPI.csproj"  \
    --configuration Release \
    --no-restore \
    --output /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
 
COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "ClinicAPI.dll"]