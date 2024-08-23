FROM hub.nexdev.net/base/dotnet/core/sdk:5.0.102-ca-patch-buster-slim AS builder
WORKDIR /src

COPY ./Fanex.COProfit.Service/Fanex.COProfit.Business ./Fanex.COProfit.Business
COPY ./Fanex.COProfit.Service/Fanex.COProfit.Service/Fanex.COProfit.Service.csproj ./Fanex.COProfit.Service/Fanex.COProfit.Service.csproj

RUN ls /src -l
WORKDIR /src/Fanex.COProfit.Service
RUN dotnet nuget add source https://nuget.nexdev.net/api/v2
RUN dotnet restore -v n

COPY ./Fanex.COProfit.Service/Fanex.COProfit.Service .

RUN dotnet publish -r linux-x64 --self-contained false -c Release -o /app

FROM hub.nexdev.net/base/dotnet/core/aspnet:5.0-nap-mail-core


# Don’t run as root
RUN adduser --disabled-password \
  --home /app \
  --gecos '' dotnetuser && chown -R dotnetuser /app
USER dotnetuser
#

ENV TZ="America/La_Paz" 

WORKDIR /app
COPY --from=builder /app .

ENTRYPOINT ["dotnet", "Fanex.COProfit.Service.dll"]
