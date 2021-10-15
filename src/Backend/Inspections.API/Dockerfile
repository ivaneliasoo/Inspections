#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Inspections.API/Inspections.API.csproj", "Inspections.API/"]
COPY ["Inspections.Infrastructure/Inspections.Infrastructure.csproj", "Inspections.Infrastructure/"]
COPY ["Inspections.Core/Inspections.Core.csproj", "Inspections.Core/"]
COPY ["Inspections.Shared/Inspections.Shared.csproj", "Inspections.Shared/"]
RUN dotnet restore "Inspections.API/Inspections.API.csproj"
COPY . .
WORKDIR /src/Inspections.API
RUN dotnet build "Inspections.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Inspections.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
RUN apt-get update && apt-get install -y libx11-6 libx11-xcb1 libatk1.0-0 libgtk-3-0 libcups2 libdrm2 libxkbcommon0 libxcomposite1 libxdamage1 libxrandr2 libgbm1 libpango-1.0-0 libcairo2 libasound2 libxshmfence1 libnss3
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Inspections.API.dll"]