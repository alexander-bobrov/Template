FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Template/Template.csproj", "Template/"]
RUN dotnet restore "Template/Template.csproj"
COPY . .
WORKDIR "/src/Template"
RUN dotnet build "Template.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Template.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Template.dll"]
