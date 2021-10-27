using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventStore.Client;

namespace ClientAPI.Context {
    public class EventStoreContext : IEventStoreContext {

        private readonly string _connectionString = "esdb://localhost:2113?tls=false&tlsVerifyCert=false";
        private readonly EventStoreClient _eventStoreClient;

        public EventStoreClient GetClient() =>
            _eventStoreClient;


        public EventStoreContext() {

            var settings = EventStoreClientSettings
                .Create(_connectionString);

            _eventStoreClient = new EventStoreClient(settings);

        }

    }
}
