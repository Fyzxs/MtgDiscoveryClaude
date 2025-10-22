# DotNet

# Linux
https://learn.microsoft.com/en-us/dotnet/core/install/linux-ubuntu-install?tabs=dotnet9&pivots=os-linux-ubuntu-2404

# Cosmos Emulator

## Docker
install docker
```
sudo apt install docker.io docker-compose
```

## Cosmos Emulator Image
```
docker pull mcr.microsoft.com/cosmosdb/linux/azure-cosmos-emulator:vnext-preview
```

To run it
```
docker run --detach --publish 8081:8081 --publish 1234:1234 mcr.microsoft.com/cosmosdb/linux/azure-cosmos-emulator:vnext-preview
```
with https
```
docker run --detach --publish 8081:8081 --publish 1234:1234 mcr.microsoft.com/cosmosdb/linux/azure-cosmos-emulator:vnext-preview --protocol https
```

see it running
```
docker ps
```
## Front End Setup
install npm
install node
install vite

## Browser
https://addons.mozilla.org/en-US/firefox/addon/apollo-developer-tools/

# Optional
```
https://ohmyposh.dev/docs/installation/customize
```