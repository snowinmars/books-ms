FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 5432
EXPOSE 443

FROM snowinmars/dotnet-python AS publish
ARG WATERMARK
WORKDIR /app
COPY . .
RUN pip3 install -r /app/_configuration/build/python/build.deps
RUN python3 /app/_scripts/build.py --config_file /app/_configuration/build/variables/Release/build-settings.json --watermark $WATERMARK

FROM base AS final
# ENV DATABASE_CONNECTION_STRING="Host=localhost;Port=5432;Database=usersdb;Username=postgres;Password=XXX"
COPY --from=publish /app/_output .
ENTRYPOINT ["dotnet", "BookService.WebApi.dll"]
