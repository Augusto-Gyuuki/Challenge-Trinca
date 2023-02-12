#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Challenge.Trinca.Web/Challenge.Trinca.Web.csproj", "Challenge.Trinca.Web/"]
COPY ["Challenge.Trinca.Application/Challenge.Trinca.Application.csproj", "Challenge.Trinca.Application/"]
COPY ["Challenge.Trinca.Domain/Challenge.Trinca.Domain.csproj", "Challenge.Trinca.Domain/"]
COPY ["Challenge.Trinca.Infrastructure/Challenge.Trinca.Infrastructure.csproj", "Challenge.Trinca.Infrastructure/"]
COPY ["Challenge.Trinca.Persistence/Challenge.Trinca.Persistence.csproj", "Challenge.Trinca.Persistence/"]
COPY ["Challenge.Trinca.Presentation/Challenge.Trinca.Presentation.csproj", "Challenge.Trinca.Presentation/"]
RUN dotnet restore "Challenge.Trinca.Web/Challenge.Trinca.Web.csproj"
COPY . .
WORKDIR "/src/Challenge.Trinca.Web"
RUN dotnet build "Challenge.Trinca.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Challenge.Trinca.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Challenge.Trinca.Web.dll"]