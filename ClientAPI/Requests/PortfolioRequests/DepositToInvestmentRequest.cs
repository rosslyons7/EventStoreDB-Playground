using System;

namespace ClientAPI.Requests.PortfolioRequests {
    public record DepositToInvestmentRequest {

        public Guid PortfolioId { get; set; }
        public string InvestmentId { get; set; }
        public double Deposit { get; set; }
    }
}
