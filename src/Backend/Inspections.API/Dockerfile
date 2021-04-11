#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["Inspections.API/Inspections.API.csproj", "Inspections.API/"]
COPY ["Inspections.Infrastructure/Inspections.Infrastructure.csproj", "Inspections.Infrastructure/"]
COPY ["Inspections.Core/Inspections.Core.csproj", "Inspections.Core/"]
COPY ["Inspections.Shared/Inspections.Shared.csproj", "Inspections.Shared/"]
RUN dotnet restore "Inspections.API/Inspections.API.csproj"
COPY . .
WORKDIR "/src/Inspections.API"
RUN dotnet build "Inspections.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Inspections.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Inspections.API.dll"]
