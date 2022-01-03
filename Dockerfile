FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /source

COPY *.sln .
COPY Server/*.csproj ./Server/
# COPY Server.Data/*.csproj ./Server.Data/
RUN dotnet restore

COPY . .
RUN dotnet publish --no-restore -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /source/out .
ENTRYPOINT ["dotnet", "Server.dll"]
