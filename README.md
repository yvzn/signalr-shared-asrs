# Shared Azure SignalR Service

A simple experiment on sharing a single Azure SignalR service between multiple applications.

## Why?

When two or more applications use Azure SignalR service for real-time messaging, one common
pattern is to create separated instances of Azure SignalR service, one for each application.

This can have some cost implications, especially for standard plans.

One possible solution is to use the same Azure SignalR service for all applications, but
then the messages can be mixed among applications or sent to the wrong users, especially
if the messages have the same name.

Fortunately Azure SignalR SDK provides an option to discriminate messages between apps,
by declaring an `ApplicationName` at startup when registering the service.

## How?

This solution consists in two apps:
- App.One
- App.Two

Each app has a server, with a SignalR hub, and a client that connects to the server's hub.

The servers sends a ping message to the client every few seconds.

The demo shows that if `ApplicationName` is not set at startup then the messages get
mixed between the two clients and servers.

If `ApplicationName` is properly configured with different values for each app, then the
messages are properly discriminated between App.One and App.Two.

## Run Locally

Requirements:
- .NET SDK >= 5
- An instance of Azure SignalR service

Set the connection strings:

```powershell
dotnet user-secrets set Azure:SignalR:ConnectionString "<Your connection string>" --project App.One/Server/

dotnet user-secrets set Azure:SignalR:ConnectionString "<Your connection string>" --project App.Two/Server/
```

Install HTTPS development certificates (if required):

```powershell
dotnet dev-certs https --trust
```

Run the 4 apps:
```powershell
'App.One/Server','App.One/Client','App.Two/Server','App.Two/Client' | % { start "cmd" "/k dotnet run" -WorkingDirectory $_ }
```

## TODO
- Sanitize AppName automatically, to match Azure SignalR requirements
  - <q>Property 'ApplicationName' value should be prefixed with alphabetic characters and only contain alpha-numeric characters or underscore.</q>
- Wait for Azure SignalR connection ready before scheduling messages
- Use Tye to configure and run the 4 apps

