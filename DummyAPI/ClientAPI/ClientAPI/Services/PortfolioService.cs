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
               configureOperationOptions: o => o.TimeoutAfter = TimeSpan.FromSeconds(15)
               );

            var events = await result.ToListAsync(cancellationToken: cancellationToken);
            var portfolio = new Portfolio();
            foreach(var @event in events) {
                var jsonObj = Encoding.UTF8.GetString(@event.Event.Data.ToArray());
                portfolio.When(jsonObj, @event.Event.EventType);
            }

            return portfolio;
        }

        public async Task<Portfolio> BuildPortfolio(Guid portfolioId, CancellationToken cancellationToken) =>
            await GetPortfolioAggregate(portfolioId, cancellationToken);


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

            await AppendDummyEventsToPortfolio(portfolioId, cancellationToken);

            return portfolioId;
        }

        private async Task AppendDummyEventsToPortfolio(Guid portfolioId, CancellationToken cancellationToken) {

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
                 cancellationToken: cancellationToken
                 );

            for(var i = 0; i < 50; i++) {
                if (i % 2 == 0) {
                    var deposit1 = PortfolioEventHandler.InvestmentDeposit("ABC", 50);
                    var deposit2 = PortfolioEventHandler.InvestmentDeposit("DEF", 100);
                    var deposit3 = PortfolioEventHandler.InvestmentDeposit("GHI", 150);
                    var deposit4 = PortfolioEventHandler.InvestmentDeposit("JKL", 200);
                    var deposit5 = PortfolioEventHandler.InvestmentDeposit("MNO", 250);
                    var deposit6 = PortfolioEventHandler.InvestmentDeposit("PQR", 300);

                    await _eventStore.AppendToStreamAsync(
                        $"portfolio-{portfolioId}",
                        StreamState.Any,
                        new[] { deposit1, deposit2, deposit3, deposit4, deposit5, deposit6 },
                        cancellationToken: cancellationToken
                        );
                }
                else {
                    var withdraw1 = PortfolioEventHandler.InvestmentWithdrawal("ABC", 25);
                    var withdraw2 = PortfolioEventHandler.InvestmentWithdrawal("DEF", 75);
                    var withdraw3 = PortfolioEventHandler.InvestmentWithdrawal("GHI", 125);
                    var withdraw4 = PortfolioEventHandler.InvestmentWithdrawal("JKL", 175);
                    var withdraw5 = PortfolioEventHandler.InvestmentWithdrawal("MNO", 225);
                    var withdraw6 = PortfolioEventHandler.InvestmentWithdrawal("PQR", 275);

                    await _eventStore.AppendToStreamAsync(
                        $"portfolio-{portfolioId}",
                        StreamState.Any,
                        new[] { withdraw1, withdraw2, withdraw3, withdraw4, withdraw5, withdraw6 },
                        cancellationToken: cancellationToken
                        );
                }

            }
        }

        

        public PortfolioService(IEventStoreContext eventStore) {

            _eventStore = eventStore.GetClient();
        }
    }
}
