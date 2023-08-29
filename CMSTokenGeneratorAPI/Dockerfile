# SDK
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS base
WORKDIR /app

# copy the csproj and restore any dependencies
COPY CMSTokenGeneratorAPI.csproj .
RUN dotnet restore

# publish 
COPY . .
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0
ENV ASPNETCORE_URL=http//+:5000
WORKDIR /app
COPY --from=base /app/publish .
ENTRYPOINT [ "dotnet", "CMSTokenGeneratorAPI.dll" ]