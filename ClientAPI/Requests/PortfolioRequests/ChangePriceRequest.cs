using System;

namespace ClientAPI.Requests.PortfolioRequests {
    public record ChangePriceRequest {

        public Guid PortfolioId { get; set; }
        public string InvestmentId { get; set; }
        public double PercentageChange { get; set; }
    }
}
