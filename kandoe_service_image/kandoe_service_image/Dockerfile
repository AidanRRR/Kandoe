FROM microsoft/dotnet:1.1.1-sdk
ENTRYPOINT ["dotnet", "kandoe_service_image.dll"]
ARG source=./bin/Debug/netcoreapp1.1/publish/
WORKDIR /app
EXPOSE 5060
COPY $source .
