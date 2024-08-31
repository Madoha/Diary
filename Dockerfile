FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Diary.Api/Diary.Api.csproj", "Diary.Api/"]
COPY ["Diary.DAL/Diary.DAL.csproj", "Diary.DAL/"]
COPY ["Diary.Domain/Diary.Domain.csproj", "Diary.Domain/"]
COPY ["Diary.Application/Diary.Application.csproj", "Diary.Application/"]
RUN dotnet restore "Diary.Api/Diary.Api.csproj"
COPY . .
WORKDIR "/src/Diary.Api"
RUN dotnet build "Diary.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Diary.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Diary.Api.dll"]
