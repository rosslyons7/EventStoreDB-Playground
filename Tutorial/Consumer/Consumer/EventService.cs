using EventStore.ClientAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consumer {
    class EventService {

        private IEventStoreConnection _connection;

        public async void Init() {
            Console.WriteLine("INIT");
            _connection = EventStoreConnection.Create(
                new Uri("tcp://admin:changeit@localhost:1113")
                );
            await _connection.ConnectAsync();
            var readEvents = await ReadEvents();

            foreach (var evt in readEvents.Events) {
                Console.WriteLine(Encoding.UTF8.GetString(evt.Event.Data));
            }
        }

        private async Task<StreamEventsSlice> ReadEvents() {

            Console.WriteLine("READ EVENTS");

            const string streamName = "newstream";
            var readEvents = await _connection.ReadStreamEventsForwardAsync(streamName, 0, 10, true);

            return readEvents;
        }

        public EventService() {

            Init();
        }
    }
}
