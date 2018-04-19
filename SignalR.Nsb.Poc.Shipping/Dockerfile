FROM microsoft/dotnet-nightly:2.1-runtime AS base
WORKDIR /app

FROM microsoft/dotnet-nightly:2.1-runtime AS build
WORKDIR /src
COPY SignalR.Nsb.Poc.sln ./
COPY SignalR.Nsb.Poc.Shipping/SignalR.Nsb.Poc.Shipping.csproj SignalR.Nsb.Poc.Shipping/
COPY SignalR.Nsb.Poc.Messages/SignalR.Nsb.Poc.Messages.csproj SignalR.Nsb.Poc.Messages/
COPY SignalR.Nsb.Poc.NServiceBus/SignalR.Nsb.Poc.NServiceBus.csproj SignalR.Nsb.Poc.NServiceBus/
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/SignalR.Nsb.Poc.Shipping
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "SignalR.Nsb.Poc.Shipping.dll"]
