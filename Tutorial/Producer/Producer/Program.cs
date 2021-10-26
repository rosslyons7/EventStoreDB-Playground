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

        static async Task<int> Deposit(EventStoreClient client, EventHandler eventHandler, string streamName, double deposit) {
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

            return 0;
        }

        static async Task<int> Withdraw(EventStoreClient client, EventHandler eventHandler, string streamName, double withdrawal) {
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

            return 0;
        }

        static async Task<string> InitClient(EventStoreClient client, EventHandler handler) {

            var streamName = await CreateClientEvent(client, new EventHandler());

            await Deposit(client, handler, streamName, 250.00);
            await Deposit(client, handler, streamName, 50.00);
            await Deposit(client, handler, streamName, 100.00);
            await Deposit(client, handler, streamName, 10.00);
            await Deposit(client, handler, streamName, 50.00);


            await Withdraw(client, handler, streamName, 50.00);
            await Withdraw(client, handler, streamName, 100.00);
            await Deposit(client, handler, streamName, 50.00);
            await Deposit(client, handler, streamName, 100.00);
            await Withdraw(client, handler, streamName, 50.00);

            return streamName;

        }

        static async Task UpdateClient(EventStoreClient client, EventHandler handler, string streamName) {


            await Deposit(client, handler, streamName, 250.00);
            await Deposit(client, handler, streamName, 50.00);
            await Deposit(client, handler, streamName, 100.00);
            await Deposit(client, handler, streamName, 10.00);
            await Deposit(client, handler, streamName, 50.00);


            await Withdraw(client, handler, streamName, 50.00);
            await Withdraw(client, handler, streamName, 100.00);
            await Deposit(client, handler, streamName, 50.00);
            await Deposit(client, handler, streamName, 100.00);
            await Withdraw(client, handler, streamName, 50.00);

        }

        static async Task Main() {


            var settings = EventStoreClientSettings
                .Create("esdb://localhost:2113?tls=false&tlsVerifyCert=false");


            var client = new EventStoreClient(settings);
            var handler = new EventHandler();
            var stream = await InitClient(client, handler);
            await UpdateClient(client, handler, streamName: stream);
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
