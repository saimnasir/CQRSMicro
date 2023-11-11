#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CQRSMicro.CustomerApp/CQRSMicro.CustomerApp.csproj", "CQRSMicro.CustomerApp/"]
COPY ["CQRSMicro.Domain/CQRSMicro.Domain.csproj", "CQRSMicro.Domain/"]
RUN dotnet restore "CQRSMicro.CustomerApp/CQRSMicro.CustomerApp.csproj"
COPY . .
WORKDIR "/src/CQRSMicro.CustomerApp"
RUN dotnet build "CQRSMicro.CustomerApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CQRSMicro.CustomerApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CQRSMicro.CustomerApp.dll"]