# Use the official .NET Core SDK as a parent image
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copy the project file and restore any dependencies
COPY *.sln .
COPY Model/*.csproj Model/
COPY Business/*.csproj Business/
COPY Data/*.csproj Data/
COPY Api/*.csproj Api/
RUN dotnet restore

# Copy the rest of the application code
COPY . .

# Publish the application (especificando directamente el proyecto)
RUN dotnet publish ContosoPizza.sln -c Release -o Api/out

# Build the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/Api/out ./

# Expose the port your application will run on
EXPOSE 80

# Start the application
ENTRYPOINT ["dotnet", "ContosoPizza.Api.dll"]