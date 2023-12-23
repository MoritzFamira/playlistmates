# Use the official image as a parent image.
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build


# Set the working directory.
WORKDIR /src

# Copy everything else and build app
COPY PlaylistMates.Application PlaylistMates.Application
COPY PlaylistMates.Webapi PlaylistMates.Webapi

RUN dotnet restore "PlaylistMates.Webapi"
RUN dotnet build "PlaylistMates.Webapi" -c Release -o /app/build
RUN dotnet publish "PlaylistMates.Webapi" -c Release -o /app/publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final

EXPOSE 5000

WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:5000

# Configure the entry point of the Docker image.
# This will use the `dotnet` command to run your application.
ENTRYPOINT ["dotnet", "PlaylistMates.Webapi.dll"]
