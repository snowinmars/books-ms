FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS publish
ARG WATERMARK
WORKDIR /app
COPY . .
RUN apt-get update
RUN apt-get install -y \
        python3 \
        python3-pip
RUN pip3 install -r /app/_configuration/build/python/build.deps
RUN python3 /app/_scripts/build.py --config_file /app/_configuration/build/variables/Debug/build-settings.json --watermark $WATERMARK

FROM base AS final
# ENV DATABASE_CONNECTION_STRING="Host=localhost;Port=5432;Database=usersdb;Username=postgres;Password=XXX"
COPY --from=publish /app/_output .
ENTRYPOINT ["dotnet", "BookService.WebApi.dll"]
