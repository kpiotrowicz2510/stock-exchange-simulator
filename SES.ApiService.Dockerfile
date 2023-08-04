FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
COPY ./SES.Shared ./SES.Shared
WORKDIR /src
COPY ./SES.ApiService .
RUN dotnet build "SES.ApiService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SES.ApiService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SES.ApiService.dll"]
