# DotNetDiagnostics.Sample Project
This is a sample project demonstrating listening .NET Runtime events for both in-process and out of process scenarios.
The API project contains a type called `GCEventListener` type. This type uses `EventListener` to subscribe to the GC events and process them.

In order to see the captured events, run the API project. 

Send approximately more than ten requests to the API(https://localhost:5001/WeatherForecast) to trigger the GC. The controller method contains large object allocations to trigger the Gen2 GC.

```bash
info: Microsoft.Hosting.Lifetime[0]
      Now listening on: https://localhost:5001
info: Microsoft.Hosting.Lifetime[0]
      Now listening on: http://localhost:5000
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
info: Microsoft.Hosting.Lifetime[0]
      Content root path: C:\Users\ilkay\Documents\GitHub\Personal\dotnet-diagnostics-sample\src\DotNetDiagnostics.Sample.API
Suspend started ticks = 637471189169853225
Suspend ended ticks = 637471189169857687
```

## Testing the out of process scenario
Keep the API project running and run the `DotNetDiagnostics.Sample.MonitorTool` project. It may be useful to run the project by running `dotnet run` command. In order to find the process ID of the API, run `dotnet run ps`. This command will display the processes having diagnostic server inside. Find the API project's pID and run `dotnet run monitor-gc-event <pID>` to process the GC events.

Example,
```bash
PS C:\Users\ilkay\Documents\GitHub\Personal\dotnet-diagnostics-sample\src\DotNetDiagnostics.Sample.MonitorTool> dotnet run ps
Process Id 25740 Name DotNetDiagnostics.Sample.API
Process Id 17308 Name dotnet
Process Id 30672 Name dotnet
Process Id 14840 Name DotNetDiagnostics.Sample.MonitorTool
PS C:\Users\ilkay\Documents\GitHub\Personal\dotnet-diagnostics-sample\src\DotNetDiagnostics.Sample.MonitorTool> dotnet run monitor-gc-events 25740
GC Started 637471302695205598
GC Started 637471302925088644
```

