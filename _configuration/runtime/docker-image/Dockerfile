FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS publish
WORKDIR /app
RUN apt-get update
RUN apt-get install -y \
        python3 \
        python3-pip