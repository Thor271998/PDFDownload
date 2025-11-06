FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source
COPY PDFDownload.sln .
COPY PDFDownload/PDFDownload.csproj PDFDownload/
COPY PDFDownloaderTestProject/PDFDownloaderTestProject.csproj PDFDownloaderTestProject/
RUN dotnet restore

COPY . .
WORKDIR /source/PDFDownload
RUN dotnet publish -c release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /out ./
COPY PDFDownload/List_Folder List_Folder
COPY PDFDownload/Output Output

ENV DOTNET_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "PDFDownload.dll"]