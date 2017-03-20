FROM microsoft/dotnet:1.1.1-sdk

COPY . /app

WORKDIR /app/API

RUN ["dotnet", "restore"]

RUN ["dotnet", "build"]

EXPOSE 5030

CMD ["dotnet", "run", "--server.urls", "http://*:5002"]
