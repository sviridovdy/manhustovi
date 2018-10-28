FROM microsoft/dotnet:2.1-aspnetcore-runtime

ENV	ASPNETCORE_URLS "http://0.0.0.0:80"

EXPOSE 80/tcp

COPY . /app 

ENTRYPOINT ["sh", "-c", "cd app && dotnet manhustovi.admin.dll"]