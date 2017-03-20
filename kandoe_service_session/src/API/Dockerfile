FROM microsoft/dotnet:1.1.1-sdk
ENTRYPOINT ["dotnet", "API.dll"]
ARG source=./bin/Debug/netcoreapp1.0/publish/
WORKDIR /app
EXPOSE 5050
COPY $source .
