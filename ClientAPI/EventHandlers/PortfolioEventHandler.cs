using ClientAPI.Models.PortfolioEvents;
using EventStore.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClientAPI.EventHandlers {
    public static class PortfolioEventHandler {

        public static EventData CreatePortfolio(Guid portfolioId) {

            var evt = new CreatePortfolio
            {
                Id = portfolioId,
                ClientDisplayName = "Dummy Client",
                ClientId = Guid.NewGuid(),
                Name = "Dummy Portfolio"
            };

            return new EventData(
                Uuid.NewUuid(),
                $"create-portfolio",
                JsonSerializer.SerializeToUtf8Bytes(evt));
        }

        public static EventData ChangePrice(string investmentId, double percentage) {
            var evt = new ChangePrice
            {
                InvestmentId = investmentId,
                PercentageChange = percentage
            };

            return new EventData(
                Uuid.NewUuid(),
                $"price-change",
                JsonSerializer.SerializeToUtf8Bytes(evt));
        }

        public static EventData CreateInvestment(string investmentId, double initialInvestment) {
            var evt = new CreateInvestment
            {
                InvestmentId = investmentId,
                InitialInvestment = initialInvestment
            };

            return new EventData(
                Uuid.NewUuid(),
                $"create-investment",
                JsonSerializer.SerializeToUtf8Bytes(evt));
        }

        public static EventData InvestmentDeposit(string investmentId, double deposit) {
            var evt = new InvestmentDeposit
            {
                InvestmentId = investmentId,
                DepositAmount = deposit
            };

            return new EventData(
                Uuid.NewUuid(),
                $"investment-deposit",
                JsonSerializer.SerializeToUtf8Bytes(evt));
        }

        public static EventData InvestmentWithdrawal(string investmentId, double withdrawal) {
            var evt = new InvestmentWithdrawal
            {
                InvestmentId = investmentId,
                WithdrawalAmount = withdrawal
            };

            return new EventData(
                Uuid.NewUuid(),
                $"investment-withdrawal",
                JsonSerializer.SerializeToUtf8Bytes(evt));
        }

        public static EventData PortfolioWithdrawal(double withdrawal) {
            var evt = new PortfolioWithdrawal
            {
                WithdrawalAmount = withdrawal
            };

            return new EventData(
                Uuid.NewUuid(),
                $"portfolio-withdrawal",
                JsonSerializer.SerializeToUtf8Bytes(evt));
        }

        public static EventData PortfolioDeposit(double deposit) {
            var evt = new PortfolioDeposit
            {
                DepositAmount = deposit
            };

            return new EventData(
                Uuid.NewUuid(),
                $"portfolio-deposit",
                JsonSerializer.SerializeToUtf8Bytes(evt));
        }

    }
}
