# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project file from subfolder
COPY ["PremiumForLearners/PremiumForLearners.csproj", "PremiumForLearners/"]
RUN dotnet restore "PremiumForLearners/PremiumForLearners.csproj"

# Copy everything else
COPY . .

# Build and publish
WORKDIR "/src/PremiumForLearners"
RUN dotnet build "PremiumForLearners.csproj" -c Release -o /app/build
RUN dotnet publish "PremiumForLearners.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy published files
COPY --from=build /app/publish .

# Render provides the PORT environment variable
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "PremiumForLearners.dll"]