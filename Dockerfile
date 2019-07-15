FROM mcr.microsoft.com/dotnet/core/sdk:2.2

WORKDIR ./BlogSite

COPY . .

RUN ls

WORKDIR ./BlogSite

RUN dotnet restore

RUN dotnet publish ./BlogSite.csproj -o /publish

WORKDIR /publish

ENTRYPOINT ["dotnet", "BlogSite.dll"]