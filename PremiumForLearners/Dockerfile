FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj from the subfolder
COPY ["PremiumForLearners/PremiumForLearners.csproj", "PremiumForLearners/"]

# Restore dependencies
RUN dotnet restore "PremiumForLearners/PremiumForLearners.csproj"

# Copy everything else
COPY . .

# Build and publish
WORKDIR "/src/PremiumForLearners"
RUN dotnet build "PremiumForLearners.csproj" -c Release -o /app/build
RUN dotnet publish "PremiumForLearners.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 1000
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PremiumForLearners.dll"]
