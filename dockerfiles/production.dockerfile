FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5184
ENV ASPNETCORE_URLS=http://+:5184

# Best practices running as a non-root user
# https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["vem.csproj", "."]
RUN dotnet restore "vem.csproj"
COPY . .

RUN dotnet build "vem.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "vem.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "vem.dll" ]