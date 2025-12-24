FROM mcr.microsoft.com/dotnet/sdk:10.0 AS dev
WORKDIR /src
CMD ["sleep", "infinity"]

FROM dev AS build
COPY . .
RUN dotnet restore "Tasks.sln"
WORKDIR "src/Runner"
RUN dotnet publish "Runner.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Runner.dll"]
