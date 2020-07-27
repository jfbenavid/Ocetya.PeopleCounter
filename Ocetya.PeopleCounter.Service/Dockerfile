#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-nanoserver-1903 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-nanoserver-1903 AS build
WORKDIR /src
COPY ["GranEstacion.Service/GranEstacion.Service.csproj", "GranEstacion.Service/"]
COPY ["GranEstacion.Repository/GranEstacion.Repository.csproj", "GranEstacion.Repository/"]
RUN dotnet restore "GranEstacion.Service/GranEstacion.Service.csproj"
COPY . .
WORKDIR "/src/GranEstacion.Service"
RUN dotnet build "GranEstacion.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GranEstacion.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GranEstacion.Service.dll"]