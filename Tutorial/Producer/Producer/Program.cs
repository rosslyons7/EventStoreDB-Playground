using EventStore.Client;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Producer {
    class Program {
        static EventData CreateSample(int i) {
            var evt = new
            {
                EntityId = Guid.NewGuid().ToString("N"),
                ImportantData = "Test Event"+i
            };

            var eventData = new EventData(
                Uuid.NewUuid(),
                "TestEvent",
                JsonSerializer.SerializeToUtf8Bytes(evt)
            );

            return eventData;
        }

        

        static async Task Main() {

            var token = new CancellationTokenSource();


            var settings = EventStoreClientSettings
                .Create("esdb://localhost:2113?tls=false");


            var client = new EventStoreClient(settings);

            await client.AppendToStreamAsync(
                "some-stream",
                StreamState.Any,
                new[] { CreateSample(1), CreateSample(2), CreateSample(3) },
                cancellationToken: token.Token
                );
            Console.WriteLine("finished");
        }
    }
}
