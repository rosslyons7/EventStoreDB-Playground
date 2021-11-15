using System;

namespace ClientAPI.Requests.PortfolioRequests {
    public record WithdrawFromPortfolioRequest {

        public Guid PortfolioId { get; set; }
        public double Withdrawal { get; set; }
    }
}
