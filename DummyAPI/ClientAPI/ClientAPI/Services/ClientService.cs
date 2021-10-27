using ClientAPI.Context;
using ClientAPI.Entities;
using ClientAPI.Models;
using ClientAPI.Responses;
using EventStore.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ClientAPI.Services {
    public class ClientService : IClientService {

        private readonly EventStoreClient _eventStore;

        public async Task<TimedGetClientResponse> GetClientById(string id, CancellationToken cancellationToken) {

            var startTime = DateTime.Now;
            var result = _eventStore.ReadStreamAsync(
                Direction.Forwards,
                $"client-{id}",
                StreamPosition.Start,
                cancellationToken: cancellationToken,
                configureOperationOptions: o => o.TimeoutAfter = TimeSpan.FromSeconds(15)
                );

            var events = await result.ToListAsync(cancellationToken: cancellationToken);
            var client = new Client();

            foreach (var @event in events) {
                var jsonObj = Encoding.UTF8.GetString(@event.Event.Data.ToArray());
                client.When(jsonObj, @event.Event.EventType);
            }

            var endTime = DateTime.Now;
            return new TimedGetClientResponse {Client = client, Time = (endTime - startTime).ToString(), EventTotal = events.Count };
        }

        public async void PushEventsToClientStream(string id, CancellationToken cancellationToken) {

            for (int i = 0; i < 100; i++) {
                var w = Withdraw(100);
                var d = Deposit(150);
                await _eventStore.AppendToStreamAsync(
                    $"client-{id}",
                    StreamState.Any,
                    new[] { w, d },
                    cancellationToken: cancellationToken
                    );
            }
        }

        private EventData Deposit(double d) {

            var evt =  new ClientInvestmentDeposit
            {
                AmountDeposited = d
            };

            return new EventData(
                Uuid.NewUuid(),
                $"investment-deposit",
                JsonSerializer.SerializeToUtf8Bytes(evt));
        }

        private EventData Withdraw(double withdrawal) {

            var evt =  new ClientInvestmentWithdrawal
            {
                AmountWithdrawn = withdrawal
            };

            return new EventData(
                Uuid.NewUuid(),
                $"investment-withdrawal",
                JsonSerializer.SerializeToUtf8Bytes(evt));
        }

        private ClientNameChange ChangeName(string firstName, string lastName) {

            return new ClientNameChange
            {
                FirstName = firstName,
                LastName = lastName
            };
        }


        public ClientService(IEventStoreContext eventStore) {

            _eventStore = eventStore.GetClient();
        }
    }
}
