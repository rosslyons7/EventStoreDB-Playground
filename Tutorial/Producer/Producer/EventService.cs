using EventStore.ClientAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Producer {
    class EventService {

        private IEventStoreConnection _connection;

        public async void Init() {
            Console.WriteLine("INIT");
            _connection = EventStoreConnection.Create(
                   new Uri("tcp://admin:changeit@localhost:1113")
            );
            await _connection.ConnectAsync();
        }

        public async void AppendEvent() {
            Console.WriteLine("Append started.");
            const string streamName = "newstream";
            const string eventType = "event-type";
            const string data = "{ \"a\":\"2\"}";
            const string metadata = "{}";

            var eventPayload = new EventData(
                eventId: Guid.NewGuid(),
                type: eventType,
                isJson: true,
                data: Encoding.UTF8.GetBytes(data),
                metadata: Encoding.UTF8.GetBytes(metadata)
            );
            await _connection.AppendToStreamAsync(streamName, ExpectedVersion.Any, eventPayload);
            Console.WriteLine("Append ended.");

        }


        public EventService() {
            Init();
        }
    }
}
