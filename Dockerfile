FROM microsoft/aspnetcore:2.0 As base

# RUN apt-get update && \
#     apt-get -qqy install \
#         libunwind8 \
#         libkrb5-3 \
#         libicu52 \
#         liblttng-ust0 \
#         libssl1.0.0 \
#         zlib1g \
#         libuuid1 && \
#     apt-get clean && \
#     rm -rf /var/lib/apt/lists/*

WORKDIR /

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY zookeeper-sf.csproj .
RUN dotnet restore zookeeper-sf.csproj
COPY . .
RUN dotnet build zookeeper-sf.csproj -c Release -o /setup

FROM build AS publish
RUN dotnet publish zookeeper-sf.csproj -c Release -o /setup -r linux-x64

FROM base AS final
WORKDIR /setup
COPY --from=publish /setup .
ADD start.sh /usr/bin/start.sh

WORKDIR /app
CMD bash /usr/bin/start.sh