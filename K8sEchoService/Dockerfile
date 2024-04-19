FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 50051
EXPOSE 8000


ENV ASPNETCORE_URLS=http://+:8000;http://+:50051

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["K8sEchoService/K8sEchoService.csproj", "K8sEchoService/"]
RUN dotnet restore "K8sEchoService/K8sEchoService.csproj"
COPY . .
WORKDIR "/src/K8sEchoService"
RUN dotnet build "K8sEchoService.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "K8sEchoService.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "K8sEchoService.dll"]
