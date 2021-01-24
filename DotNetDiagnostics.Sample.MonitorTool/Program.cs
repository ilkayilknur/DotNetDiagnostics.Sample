using Microsoft.Diagnostics.NETCore.Client;
using Microsoft.Diagnostics.Tracing;
using System;
using System.Diagnostics;
using System.Diagnostics.Tracing;

namespace DotNetDiagnostics.Sample.MonitorTool
{
    class Program
    {
        static void Main(string[] args)
        {
            switch (args[0])
            {
                case "ps":
                    ListProcesses();
                    break;
                case "monitor-gc-events":
                    MonitorGCEvents(int.Parse(args[1]));
                    break;
                default:
                    break;
            }
        }

        private static void MonitorGCEvents(int processId)
        {
            var client = new DiagnosticsClient(processId);

            using var session = client.StartEventPipeSession(new EventPipeProvider("Microsoft-Windows-DotNETRuntime", EventLevel.Informational, 1));
            EventPipeEventSource source = new EventPipeEventSource(session.EventStream);

            source.Clr.GCStart += Clr_GCStart;

            source.Process();
        }

        private static void Clr_GCStart(Microsoft.Diagnostics.Tracing.Parsers.Clr.GCStartTraceData obj)
        {
            Console.WriteLine($"GC Started {obj.TimeStamp.Ticks}");
        }

        private static void ListProcesses()
        {
            foreach (var processId in DiagnosticsClient.GetPublishedProcesses())
            {
                Console.WriteLine($"Process Id {processId} Name {Process.GetProcessById(processId).ProcessName}");
            }
        }
    }
}
