#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the .NET SDK image for the build stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy the .csproj file and restore dependencies
COPY ["backend/TrackingAPIsV1/ProductTrackingAPI/ProductTrackingAPI.csproj", "ProductTrackingAPI/"]
RUN dotnet restore "ProductTrackingAPI/ProductTrackingAPI.csproj"

# Copy the entire project and build it
COPY . .
WORKDIR "/src/backend/TrackingAPIsV1/ProductTrackingAPI"
RUN dotnet build "ProductTrackingAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ProductTrackingAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish

# Final stage: use the runtime image and copy the published files
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProductTrackingAPI.dll"]