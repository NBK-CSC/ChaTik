FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ChaTik/ChaTik.csproj", "ChaTik/"]
RUN dotnet restore "ChaTik/ChaTik.csproj"
COPY . .
WORKDIR "/src/ChaTik"
RUN dotnet build "ChaTik.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ChaTik.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChaTik.dll"]
