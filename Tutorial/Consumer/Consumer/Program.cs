
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

            var startTime = DateTime.Now;

            var result = client.ReadStreamAsync(
                Direction.Forwards,
                "client-a03fe6e4-2118-42a5-830e-a2cea0252a5b",
                StreamPosition.Start,
                cancellationToken: cancellationToken);

            var events = await result.ToListAsync(cancellationToken);

            var clientObject = new Client();
            
            foreach (var @event in events) {
                var jsonObj = Encoding.UTF8.GetString(@event.Event.Data.ToArray());
                clientObject.When(jsonObj, @event.Event.EventType);
            }

            Console.WriteLine($"Investment Total: {clientObject.InvestmentTotal}");

            var endTime = DateTime.Now;
            Console.WriteLine($"Time to consume {events.Count} messages: {endTime - startTime}");
        }
    }
}
