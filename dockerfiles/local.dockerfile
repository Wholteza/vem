FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

EXPOSE 5184
ENV ASPNETCORE_URLS=http://+:5184
ENTRYPOINT [ "dotnet", "watch", "--project", "./Vem" ]