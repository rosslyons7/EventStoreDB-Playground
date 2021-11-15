using System;

namespace ClientAPI.Requests.PortfolioRequests {
    public record CreateInvestmentRequest {

        public Guid PortfolioId { get; set; }
        public string InvestmentId { get; set; }
        public double InitialInvestment { get; set; }
    }
}
