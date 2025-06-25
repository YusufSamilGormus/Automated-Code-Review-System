
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"


FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src


COPY . .


RUN dotnet restore "CodeReviewAPI.csproj"
RUN dotnet build "CodeReviewAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CodeReviewAPI.csproj" -c Release -o /app/publish


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CodeReviewAPI.dll"]
