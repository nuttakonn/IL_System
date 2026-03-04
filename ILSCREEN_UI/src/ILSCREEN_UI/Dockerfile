FROM mcr.microsoft.com/dotnet/sdk:.0

RUN apt-get update \
    && DEBIAN_FRONTEND=noninteractive apt-get install -y --no-install-recommends ca-certificates

# hadolint ignore=DL#3008,DL#3009
RUN apt-get update \
    && apt-get install -y --no-install-recommends libunwind-dev

# hadolint ignore=DL#3008,DL#3009
RUN apt-get update \
    && apt-get install -y --no-install-recommends vim

# hadolint ignore=DL#3008,DL#3009
RUN apt-get update \
    && apt-get install -y --no-install-recommends telnet
    
# hadolint ignore=DL#3008,DL#3009
RUN apt-get update \
    && apt-get install -y --no-install-recommends curl

WORKDIR /app 

COPY ./bin/Release/net9.0/linux-x64/publish ./
ENTRYPOINT ["dotnet", "/app/ILSCREEN_UI.dll"]