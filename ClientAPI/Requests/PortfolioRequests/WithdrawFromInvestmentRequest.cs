using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientAPI.Requests.PortfolioRequests {
    public record WithdrawFromInvestmentRequest {

        public Guid PortfolioId { get; set; }
        public string InvestmentId { get; set; }
        public double Withdrawal { get; set; }
    }
}
