FROM microsoft/dotnet:2.1-aspnetcore-runtime

ENV	ASPNETCORE_URLS "http://0.0.0.0:80;https://0.0.0.0:443"

EXPOSE 80/tcp
EXPOSE 443/tcp

COPY . /app 

ENTRYPOINT ["sh", "-c", "cd app && dotnet manhustovi.app.dll"]