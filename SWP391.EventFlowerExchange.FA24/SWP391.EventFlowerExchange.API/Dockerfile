#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0-nanoserver-1809 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SWP391.EventFlowerExchange.API/SWP391.EventFlowerExchange.API.csproj", "SWP391.EventFlowerExchange.API/"]
COPY ["SWP391.EventFlowerExchange.Application/SWP391.EventFlowerExchange.Application.csproj", "SWP391.EventFlowerExchange.Application/"]
COPY ["SWP391.EventFlowerExchange.Infrastructure/SWP391.EventFlowerExchange.Infrastructure.csproj", "SWP391.EventFlowerExchange.Infrastructure/"]
COPY ["SWP391.EventFlowerExchange.Domain/SWP391.EventFlowerExchange.Domain.csproj", "SWP391.EventFlowerExchange.Domain/"]
RUN dotnet restore "./SWP391.EventFlowerExchange.API/SWP391.EventFlowerExchange.API.csproj"
COPY . .
WORKDIR "/src/SWP391.EventFlowerExchange.API"
RUN dotnet build "./SWP391.EventFlowerExchange.API.csproj" -c %BUILD_CONFIGURATION% -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SWP391.EventFlowerExchange.API.csproj" -c %BUILD_CONFIGURATION% -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SWP391.EventFlowerExchange.API.dll"]