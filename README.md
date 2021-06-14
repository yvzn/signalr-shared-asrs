# Shared Azure SignalR Service

A simple experiment on
- sharing a single Azure SignalR Service between multiple applications
- doing the necessary setup not to mix messages between apps.

This allows to mitigate the cost implications of having multiple Azure SignalR Services,
one for every applications.

## TODO

Complete this documentation

## Quick gists

```powershell
dotnet user-secrets set Azure:SignalR:ConnectionString "<Your connection string>"

dotnet dev-certs https --trust
```
