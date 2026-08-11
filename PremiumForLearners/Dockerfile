# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project file
COPY ["PremiumForLearners/PremiumForLearners.csproj", "PremiumForLearners/"]

# Restore dependencies
RUN dotnet restore "PremiumForLearners/PremiumForLearners.csproj"

# Copy the rest of the application
COPY . .

# Move into project directory
WORKDIR "/src/PremiumForLearners"

# Build
RUN dotnet build "PremiumForLearners.csproj" -c Release -o /app/build

# Publish
RUN dotnet publish "PremiumForLearners.csproj" -c Release -o /app/publish /p:UseAppHost=false


# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Render/hosting platforms commonly provide PORT dynamically
EXPOSE 1000

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "PremiumForLearners.dll"]
