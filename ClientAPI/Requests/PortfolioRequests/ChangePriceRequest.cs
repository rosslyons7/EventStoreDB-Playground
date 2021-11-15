using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientAPI.Requests.PortfolioRequests {
    public record ChangePriceRequest {

        public Guid PortfolioId { get; set; }
        public string InvestmentId { get; set; }
        public double PercentageChange { get; set; }
    }
}
