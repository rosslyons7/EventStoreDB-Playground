using ClientAPI.Models.PortfolioEvents;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientAPI.Entities {
    public class Portfolio {

        public Guid Id { get; set; }
        public string Name { get; set; }
        public double TotalValue { get; set; }
        public string ClientName { get; set; }
        public Guid ClientId { get; set; }
        public List<Investment> Investments { get; set; } = new List<Investment>();

        public void When(string jsonObj, string eventType) {
            switch (eventType) {
                case "change-price":
                    Apply(JsonConvert.DeserializeObject<ChangePrice>(jsonObj));
                    break;
                case "investment-deposit":
                    Apply(JsonConvert.DeserializeObject<InvestmentDeposit>(jsonObj));
                    break;
                case "investment-withdrawal":
                    Apply(JsonConvert.DeserializeObject<InvestmentWithdrawal>(jsonObj));
                    break;
                case "create-investment":
                    Apply(JsonConvert.DeserializeObject<CreateInvestment>(jsonObj));
                    break;
                case "create-portfolio":
                    Apply(JsonConvert.DeserializeObject<CreatePortfolio>(jsonObj));
                    break;
            }
        }

        public void When(object @event) {
            switch (@event) {
                case ChangePrice changePrice:
                    Apply(changePrice);
                    break;
                case InvestmentDeposit investmentDeposit:
                    Apply(investmentDeposit);
                    break;
                case InvestmentWithdrawal investmentWithdrawal:
                    Apply(investmentWithdrawal);
                    break;
                case CreateInvestment createInvestment:
                    Apply(createInvestment);
                    break;
                case CreatePortfolio createPortfolio:
                    Apply(createPortfolio);
                    break;
            }
        }

        private void Apply(ChangePrice @event) {

            var investment = Investments.SingleOrDefault(i => i.Id == @event.InvestmentId);

            if (investment == null) {
                throw new Exception("Investment not found.");
            }

            investment.HoldingValue *= @event.PercentageChange;
        }

        private void Apply(CreateInvestment @event) {

            if(Investments.Any(i => i.Id == @event.InvestmentId)) {
                throw new Exception($"Investment with Id {@event.InvestmentId} already exists.");
            }

            Investments.Add(
                new Investment
                {
                    Id = @event.InvestmentId,
                    HoldingValue = @event.InitialInvestment
                });

            TotalValue += @event.InitialInvestment;

        }
        
        private void Apply(CreatePortfolio @event) {

            if(@event.Id == Guid.Empty || @event.Name == null || @event.ClientId == Guid.Empty || @event.ClientDisplayName == null) {
                throw new Exception($"Incomplete portfolio for {@event.Id}");
            }

            Id = @event.Id;
            Name = @event.Name;
            ClientName = @event.ClientDisplayName;
            ClientId = @event.ClientId;
        }

        private void Apply(InvestmentDeposit @event) {

            var investment = Investments.SingleOrDefault(i => i.Id == @event.InvestmentId);

            if (investment == null) throw new Exception($"No investment with Id {@event.InvestmentId} exists in this portfolio.");

            if (@event.DepositAmount <= 0.00) throw new Exception($"Investment deposit cannot be lower than 0. (Attempted Deposit = {@event.DepositAmount})");

            investment.HoldingValue += @event.DepositAmount;
            TotalValue += @event.DepositAmount;

        }

        private void Apply(InvestmentWithdrawal @event) {

            var investment = Investments.SingleOrDefault(i => i.Id == @event.InvestmentId);

            if (investment == null) throw new Exception($"No investment with Id {@event.InvestmentId} exists in this portfolio.");

            investment.HoldingValue -= @event.WithdrawalAmount;
            TotalValue -= @event.WithdrawalAmount;

        }
    }
}
