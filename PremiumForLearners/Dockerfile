FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["PremiumForLearners/PremiumForLearners.csproj", "PremiumForLearners/"]

RUN dotnet restore "PremiumForLearners/PremiumForLearners.csproj"

COPY . .

WORKDIR "/src/PremiumForLearners"

RUN dotnet build "PremiumForLearners.csproj" -c Release -o /app/build

RUN dotnet publish "PremiumForLearners.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:10000

EXPOSE 10000

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "PremiumForLearners.dll"]
