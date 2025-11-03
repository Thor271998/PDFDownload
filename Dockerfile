FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source
COPY *.csproj .
RUN dotnet restore

COPY . ./app
WORKDIR /source/app
RUN dotnet publish -c release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /source/app
COPY --from=build /out ./
COPY List_Folder List_Folder
COPY Output Output
ENTRYPOINT ["dotnet", "PDFDownload.dll"]