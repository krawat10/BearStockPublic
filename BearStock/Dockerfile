FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["BearStock/BearStock.Web.csproj", "BearStock/"]
RUN dotnet restore "BearStock/BearStock.Web.csproj"
COPY . .
WORKDIR "/src/BearStock"
RUN dotnet build "BearStock.Web.csproj" -c Release -o /app/build
RUN apt-get update -yq \
    && apt-get install curl gnupg -yq \
    && curl -sL https://deb.nodesource.com/setup_10.x | bash \
    && apt-get install nodejs -yq

FROM build AS publish
RUN dotnet publish "BearStock.Web.csproj" -c Release -o /app/publish


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BearStock.Web.dll"]
