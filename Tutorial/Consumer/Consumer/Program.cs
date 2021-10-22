
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
                .Create("esdb://localhost:2113?tls=false");


            var client = new EventStoreClient(settings);

            var result = client.ReadStreamAsync(
                Direction.Forwards,
                "some-stream",
                StreamPosition.Start,
                cancellationToken: cancellationToken);

            var events = await result.ToListAsync(cancellationToken);

            foreach (var @event in events) {
                Console.WriteLine(Encoding.UTF8.GetString(@event.Event.Data.ToArray()));
            }
        }
    }
}
