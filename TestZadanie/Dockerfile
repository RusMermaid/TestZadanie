FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["TestZadanie/TestZadanie.csproj", "TestZadanie/"]
RUN dotnet restore "TestZadanie/TestZadanie.csproj"
COPY . .
WORKDIR "/src/TestZadanie"
RUN dotnet build "TestZadanie.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TestZadanie.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TestZadanie.dll"]
