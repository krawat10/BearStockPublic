#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
RUN apk add --no-cache icu-libs

FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build
WORKDIR /src
COPY ["BearStock.FinancialModelingPrep/BearStock.FinancialModelingPrep.csproj", "BearStock.FinancialModelingPrep/"]
COPY ["BearStock.Tools/BearStock.Tools.csproj", "BearStock.Tools/"]
RUN dotnet restore "BearStock.FinancialModelingPrep/BearStock.FinancialModelingPrep.csproj"
COPY . .
WORKDIR "/src/BearStock.FinancialModelingPrep"
RUN dotnet build "BearStock.FinancialModelingPrep.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BearStock.FinancialModelingPrep.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BearStock.FinancialModelingPrep.dll"]