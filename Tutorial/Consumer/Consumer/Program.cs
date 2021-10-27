
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
                "client-fbe82c4d-18e1-41dc-bbf0-eb747fcba5dd",
                StreamPosition.Start,
                cancellationToken: cancellationToken);

            var events = await result.ToListAsync(cancellationToken);

            var clientObject = new Client();
            
            foreach (var @event in events) {
                var jsonObj = Encoding.UTF8.GetString(@event.Event.Data.ToArray());
                clientObject.When(jsonObj, @event.Event.EventType);
            }

            Console.WriteLine($"Client Name: {clientObject.FirstName} {clientObject.LastName}");
            Console.WriteLine($"Investment Total: {clientObject.InvestmentTotal}");

            var endTime = DateTime.Now;
            Console.WriteLine($"Time to consume {events.Count} messages: {endTime - startTime}");
        }
    }
}
