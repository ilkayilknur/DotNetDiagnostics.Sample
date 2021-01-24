using System;
using System.Diagnostics.Tracing;

namespace DotNetDiagnostics.Sample.API
{
    public class GCEventListener : EventListener
    {
        protected override void OnEventSourceCreated(EventSource eventSource)
        {
            if (eventSource.Name == "Microsoft-Windows-DotNETRuntime")
            {
                EnableEvents(eventSource, EventLevel.Informational, (EventKeywords)0x1);
            }
        }

        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            if (eventData.EventName == "GCStart_V2")
            {
                if ((uint)eventData.Payload[2] == 4)
                {
                    //Send alarm GC triggered due to large object heap allocation.
                }
            }
            else if (eventData.EventName == "GCSuspendEEBegin_V1")
            {
                Console.WriteLine($"Suspend started ticks = {eventData.TimeStamp.Ticks}");
            }
            else if (eventData.EventName == "GCSuspendEEEnd_V1")
            {
                Console.WriteLine($"Suspend ended ticks = {eventData.TimeStamp.Ticks}");
            }
        }
    }
}
