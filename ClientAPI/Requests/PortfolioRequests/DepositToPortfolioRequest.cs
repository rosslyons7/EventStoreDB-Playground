using System;

namespace ClientAPI.Requests.PortfolioRequests {
    public record DepositToPortfolioRequest {

        public Guid PortfolioId { get; set; }
        public double Deposit { get; set; }
    }
}
