FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

ENV ASPNETCORE_URLS=http://+:8080

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["SeeviceProvider-PL/SeeviceProvider-PL.csproj", "SeeviceProvider-PL/"]
RUN dotnet restore "SeeviceProvider-PL/SeeviceProvider-PL.csproj"
COPY . .
WORKDIR "/src/SeeviceProvider-PL"
RUN dotnet build "SeeviceProvider-PL.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "SeeviceProvider-PL.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SeeviceProvider-PL.dll"]
