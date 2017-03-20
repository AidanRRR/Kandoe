FROM microsoft/dotnet:latest
ENTRYPOINT ["dotnet", "API.dll"]
ARG source=./bin/Debug/netcoreapp1.0/publish/
WORKDIR /app
EXPOSE 5030
COPY $source .
