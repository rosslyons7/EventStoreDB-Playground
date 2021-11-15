using ClientAPI.Context;
using EventStore.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ClientAPI.Subscriptions {
    public class PersistentSubscription : BackgroundService {

        private readonly EventStoreClient _client;
        private DateTime startTime;
        private int events = 0;
        protected override Task ExecuteAsync(CancellationToken stoppingToken) {

            Consume();
            return Task.CompletedTask;
        }

        private async void Consume() {

            startTime = DateTime.Now;
            await _client.SubscribeToAllAsync(
                async (subscription, evnt, cancellationToken) => {
                    events += 1;
                    Console.WriteLine($"Time taken to process {events} events -- {DateTime.Now - startTime}");
                    await HandleEvent(evnt);
                });

            Console.WriteLine("Finished");
        }

        private async Task HandleEvent(ResolvedEvent evnt) {
            await Task.Delay(0);
        }

        public PersistentSubscription(IServiceProvider serviceProvider) {
            var eventStoreContext = serviceProvider.GetRequiredService<IEventStoreContext>();
            _client = eventStoreContext.GetClient();

        }
    }
}
