
using EventStore.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Consumer {
    class Program {
        static async Task Main() {


            var cancellationToken = new CancellationTokenSource().Token;


            var settings = EventStoreClientSettings
                .Create("esdb://localhost:2113?tls=false&tlsVerifyCert=false");


            var client = new EventStoreClient(settings);
            var streamName = "portfolio-b3d52251-7374-4503-add6-3c79bae07166";

            await client.SubscribeToStreamAsync(streamName,
                async (subscription, evnt, cancellationToken) =>
                {
                    Console.WriteLine($"Received event {evnt.OriginalEventNumber}@{evnt.OriginalStreamId}");
                    await HandleEvent(evnt);
                });

            Console.WriteLine("Finished");
        }

        static async Task HandleEvent(ResolvedEvent evnt) {
            await Task.Delay(0);
        }
    }
}
