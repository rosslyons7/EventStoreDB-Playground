using EventStore.Client;
using Producer.Models;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Producer {
    class Program {

        static async Task<string> CreateClientEvent(EventStoreClient client, EventHandler eventHandler) {

            var token = new CancellationTokenSource();


            var evt = eventHandler.CreateClientEvent("Ross", "Lyons", new DateTime(2000, 1, 10), 100.00);
           

            var eventData = new EventData(
                Uuid.NewUuid(),
                $"create-client",
                JsonSerializer.SerializeToUtf8Bytes(evt));

            await client.AppendToStreamAsync(
              $"client-{evt.Id}",
              StreamState.Any,
              new[] { eventData },
              cancellationToken: token.Token
              );

            return $"client-{evt.Id}";
        }

        static async void Deposit(EventStoreClient client, EventHandler eventHandler, string streamName, double deposit) {
            var token = new CancellationTokenSource();


            var evt = eventHandler.Invest(deposit);
        

            var eventData = new EventData(
                Uuid.NewUuid(),
                $"investment-deposit",
                JsonSerializer.SerializeToUtf8Bytes(evt));

            await client.AppendToStreamAsync(
              streamName,
              StreamState.Any,
              new[] { eventData },
              cancellationToken: token.Token
              );

        }

        static async void Withdraw(EventStoreClient client, EventHandler eventHandler, string streamName, double withdrawal) {
            var token = new CancellationTokenSource();


            var evt = eventHandler.Withdraw(withdrawal);

            var eventData = new EventData(
                Uuid.NewUuid(),
                $"investment-withdrawal",
                JsonSerializer.SerializeToUtf8Bytes(evt));

            await client.AppendToStreamAsync(
              streamName,
              StreamState.Any,
              new[] { eventData },
              cancellationToken: token.Token
              );

        }

        static async void ChangeName(EventStoreClient client, EventHandler eventHandler, string streamName, string firstName, string lastName) {
            var token = new CancellationTokenSource();


            var evt = eventHandler.ChangeName(firstName, lastName);

            var eventData = new EventData(
                Uuid.NewUuid(),
                $"change-name",
                JsonSerializer.SerializeToUtf8Bytes(evt));

            await client.AppendToStreamAsync(
              streamName,
              StreamState.Any,
              new[] { eventData },
              cancellationToken: token.Token
              );

        }

        static async Task<string> InitClient(EventStoreClient client, EventHandler handler) {

            var streamName = await CreateClientEvent(client, new EventHandler());

            Deposit(client, handler, streamName, 250.00);
            Deposit(client, handler, streamName, 50.00);
            Deposit(client, handler, streamName, 100.00);
            Deposit(client, handler, streamName, 10.00);
            Deposit(client, handler, streamName, 50.00);


            Withdraw(client, handler, streamName, 50.00);
            Withdraw(client, handler, streamName, 100.00);
            Deposit(client, handler, streamName, 50.00);
            Deposit(client, handler, streamName, 100.00);
            Withdraw(client, handler, streamName, 50.00);

            return streamName;

        }

        static async Task UpdateClient(EventStoreClient client, EventHandler handler, string streamName) {

            for (int i = 0; i < 5000; i++) {
                Deposit(client, handler, streamName, 250.00);
                Withdraw(client, handler, streamName, 50.00);
            }

            ChangeName(client, handler, streamName, "David", "Lazaro");


        }

        static async Task Main() {


            var settings = EventStoreClientSettings
                .Create("esdb://localhost:2113?tls=false&tlsVerifyCert=false");

            var client = new EventStoreClient(settings);
            var handler = new EventHandler();
            //var stream = await InitClient(client, handler);
            var start = DateTime.Now;
            await UpdateClient(client, handler, streamName: "client-fbe82c4d-18e1-41dc-bbf0-eb747fcba5dd");
            var end = DateTime.Now;
            Console.WriteLine($"Time taken to produce 10,001 updates: {end-start}");

        }
    }

    class EventHandler {

        public ClientCreated CreateClientEvent(string firstName, string lastName, DateTime birthday, double deposit) {

            return new ClientCreated
            {
                Id = Guid.NewGuid(),
                FirstName = firstName,
                LastName = lastName,
                Birthday = birthday,
                DateJoined = DateTime.Today,
                InvestmentTotal = deposit,
                Email = $"{firstName}.{lastName}@not_real_email.co.uk",
                Username = $"{firstName}.{lastName}",
                Password = "MyFirstPassword123!"
            };
        }


        public ClientInvestmentDeposit Invest(double deposit) {

            return new ClientInvestmentDeposit
            {
                AmountDeposited = deposit
            };
        }

        public ClientInvestmentWithdrawal Withdraw(double withdrawal) {

            return new ClientInvestmentWithdrawal
            {
                AmountWithdrawn = withdrawal
            };
        }

        public ClientNameChange ChangeName(string firstName, string lastName) {

            return new ClientNameChange
            {
                FirstName = firstName,
                LastName = lastName
            };
        }
    }
}
