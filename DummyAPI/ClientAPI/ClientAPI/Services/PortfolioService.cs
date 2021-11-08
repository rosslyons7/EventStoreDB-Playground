using ClientAPI.Context;
using ClientAPI.Entities;
using ClientAPI.EventHandlers;
using ClientAPI.Models.PortfolioEvents;
using ClientAPI.Requests.PortfolioRequests;
using EventStore.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClientAPI.Services {
    public class PortfolioService : IPortfolioService {

        private readonly EventStoreClient _eventStore;

        private async Task<Portfolio> GetPortfolioAggregate(Guid portfolioId, CancellationToken cancellationToken) {

            var result = _eventStore.ReadStreamAsync(
               Direction.Forwards,
               $"portfolio-{portfolioId}",
               StreamPosition.Start,
               cancellationToken: cancellationToken,
               configureOperationOptions: o => o.TimeoutAfter = TimeSpan.FromSeconds(35)
               );

            var events = await result.ToListAsync(cancellationToken: cancellationToken);
            var portfolio = new Portfolio();
            foreach(var @event in events) {
                var jsonObj = Encoding.UTF8.GetString(@event.Event.Data.ToArray());
                portfolio.When(jsonObj, @event.Event.EventType);
            }

            return portfolio;
        }

        public async Task<Portfolio> BuildPortfolio(Guid portfolioId, CancellationToken cancellationToken) {
            Portfolio result = new();

            var totalTime = new TimeSpan();
            for(int i=0; i < 50; i++) {
                var start = DateTime.Now;
                result = await GetPortfolioAggregate(portfolioId, cancellationToken);
                var end = DateTime.Now;
                var timeTaken = end - start;
                totalTime += timeTaken;
            }

            Console.WriteLine($"Time taken: {totalTime / 50}");
            return result;
        }
            


        public async Task ChangePrice(ChangePriceRequest request, CancellationToken cancellationToken) {

            var portfolio = await GetPortfolioAggregate(request.PortfolioId, cancellationToken);
            portfolio.When(new ChangePrice
            {
                InvestmentId = request.InvestmentId,
                PercentageChange = request.PercentageChange
            });

            await _eventStore.AppendToStreamAsync(
                $"portfolio-{request.PortfolioId}",
                StreamState.Any,
                new[] { PortfolioEventHandler.ChangePrice(request.InvestmentId, request.PercentageChange) },
                cancellationToken: cancellationToken
                );
        }

        public async Task CreateInvestment(CreateInvestmentRequest request, CancellationToken cancellationToken) {

            var portfolio = await GetPortfolioAggregate(request.PortfolioId, cancellationToken);
            portfolio.When(new CreateInvestment
            {
                InvestmentId = request.InvestmentId,
                InitialInvestment = request.InitialInvestment
            });

            await _eventStore.AppendToStreamAsync(
                $"portfolio-{request.PortfolioId}",
                StreamState.Any,
                new[] { PortfolioEventHandler.InvestmentDeposit(request.InvestmentId, request.InitialInvestment) },
                cancellationToken: cancellationToken
                );
        }

        public async Task DepositToInvestment(DepositToInvestmentRequest request, CancellationToken cancellationToken) {

                var portfolio = await GetPortfolioAggregate(request.PortfolioId, cancellationToken);
                portfolio.When(new InvestmentDeposit
                {
                    InvestmentId = request.InvestmentId,
                    DepositAmount = request.Deposit
                });

                await _eventStore.AppendToStreamAsync(
                $"portfolio-{request.PortfolioId}",
                StreamState.Any,
                new[] { PortfolioEventHandler.InvestmentDeposit(request.InvestmentId, request.Deposit) },
                cancellationToken: cancellationToken
                );
          
        }

        public async Task WithdrawFromInvestment(WithdrawFromInvestmentRequest request, CancellationToken cancellationToken) {

            var portfolio = await GetPortfolioAggregate(request.PortfolioId, cancellationToken);
            portfolio.When(new InvestmentWithdrawal
            {
                InvestmentId = request.InvestmentId,
                WithdrawalAmount = request.Withdrawal
            });

            await _eventStore.AppendToStreamAsync(
            $"portfolio-{request.PortfolioId}",
            StreamState.Any,
            new[] { PortfolioEventHandler.InvestmentWithdrawal(request.InvestmentId, request.Withdrawal) },
            cancellationToken: cancellationToken
            );
        }

        public async Task DepositToPortfolio(DepositToPortfolioRequest request, CancellationToken cancellationToken) {
            var portfolio = await GetPortfolioAggregate(request.PortfolioId, cancellationToken);
            portfolio.When(new PortfolioDeposit
            {
                DepositAmount = request.Deposit
            });

            await _eventStore.AppendToStreamAsync(
            $"portfolio-{request.PortfolioId}",
            StreamState.Any,
            new[] { PortfolioEventHandler.PortfolioDeposit(request.Deposit) },
            cancellationToken: cancellationToken
            );
        }

        public async Task WithdrawFromPortfolio(WithdrawFromPortfolioRequest request, CancellationToken cancellationToken) {
            var portfolio = await GetPortfolioAggregate(request.PortfolioId, cancellationToken);
            portfolio.When(new PortfolioWithdrawal
            {
                WithdrawalAmount = request.Withdrawal
            });

            await _eventStore.AppendToStreamAsync(
            $"portfolio-{request.PortfolioId}",
            StreamState.Any,
            new[] { PortfolioEventHandler.PortfolioWithdrawal(request.Withdrawal) },
            cancellationToken: cancellationToken
            );
        }

        public async Task<Guid> CreatePortfolio(CancellationToken cancellationToken) {

            var portfolioId = Guid.NewGuid();

            await _eventStore.AppendToStreamAsync(
                $"portfolio-{portfolioId}",
                StreamState.Any,
                new[] { PortfolioEventHandler.CreatePortfolio(portfolioId) },
                cancellationToken: cancellationToken
                );

            var investment1 = PortfolioEventHandler.CreateInvestment("ABC", 50);
            var investment2 = PortfolioEventHandler.CreateInvestment("DEF", 100);
            var investment3 = PortfolioEventHandler.CreateInvestment("GHI", 150);
            var investment4 = PortfolioEventHandler.CreateInvestment("JKL", 200);
            var investment5 = PortfolioEventHandler.CreateInvestment("MNO", 250);
            var investment6 = PortfolioEventHandler.CreateInvestment("PQR", 300);

            await _eventStore.AppendToStreamAsync(
                 $"portfolio-{portfolioId}",
                 StreamState.Any,
                 new[] { investment1, investment2, investment3, investment4, investment5, investment6 },
                 cancellationToken: cancellationToken,
                 configureOperationOptions: o => o.TimeoutAfter = TimeSpan.FromSeconds(15)
                 );

            for(int i = 0; i < 1; i++) {
                _ = AppendDummyEventsToPortfolio(portfolioId, 5, cancellationToken);
            }
            



            return portfolioId;
        }

        public async Task PushEvents(Guid portfolioId, int events, int iterations, CancellationToken cancellationToken) {

            var total = new TimeSpan();
            for (int i = 0; i < iterations; i++) {
                total += await AppendDummyEventsToPortfolio(portfolioId, events, cancellationToken);
            }

            Console.WriteLine($"Average time taken to append {events*2} events: {total / iterations}");
        }

        private async Task<TimeSpan> AppendDummyEventsToPortfolio(Guid portfolioId, int events, CancellationToken cancellationToken) {

            var eventList = GetEventList(events).ToArray();
            var start = DateTime.Now;

            await _eventStore.AppendToStreamAsync(
                    $"portfolio-{portfolioId}",
                    StreamState.Any,
                    eventList,
                    cancellationToken: cancellationToken,
                    configureOperationOptions: o => o.TimeoutAfter = TimeSpan.FromMinutes(2)
                    );
            var end = DateTime.Now;

            return end - start;
        }

        private List<EventData> GetEventList(int events) {

            var list = new List<EventData>();

            for(int i=0; i < events; i++) {
                switch(i % 6) {
                    case 5:
                        list.Add(PortfolioEventHandler.InvestmentDeposit("ABC", 50));
                        list.Add(PortfolioEventHandler.InvestmentWithdrawal("ABC", 25));
                        break;
                    case 4:
                        list.Add(PortfolioEventHandler.InvestmentDeposit("DEF", 60));
                        list.Add(PortfolioEventHandler.InvestmentWithdrawal("DEF", 25));
                        break;
                    case 3:
                        list.Add(PortfolioEventHandler.InvestmentDeposit("GHI", 70));
                        list.Add(PortfolioEventHandler.InvestmentWithdrawal("GHI", 25));
                        break;
                    case 2:
                        list.Add(PortfolioEventHandler.InvestmentDeposit("JKL", 80));
                        list.Add(PortfolioEventHandler.InvestmentWithdrawal("JKL", 25));
                        break;
                    case 1:
                        list.Add(PortfolioEventHandler.InvestmentDeposit("MNO", 90));
                        list.Add(PortfolioEventHandler.InvestmentWithdrawal("MNO", 25));
                        break;
                    case 0:
                        list.Add(PortfolioEventHandler.InvestmentDeposit("PQR", 100));
                        list.Add(PortfolioEventHandler.InvestmentWithdrawal("PQR", 25));
                        break;
                }
            }

            return list;
        }
        

        public PortfolioService(IEventStoreContext eventStore) {

            _eventStore = eventStore.GetClient();
        }
    }
}
