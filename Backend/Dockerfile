# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY Backend.csproj .
RUN dotnet restore

# Copy everything else and build
COPY . .
WORKDIR /src
RUN dotnet build Backend.csproj -c Release -o /app/build

# Publish stage
FROM build AS publish
WORKDIR /src
RUN dotnet publish Backend.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Copy published app
COPY --from=publish /app/publish .

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Development

ENTRYPOINT ["dotnet", "Backend.dll"]

