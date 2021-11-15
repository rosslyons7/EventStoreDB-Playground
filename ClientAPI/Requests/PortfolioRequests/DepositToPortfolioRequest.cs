using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientAPI.Requests.PortfolioRequests {
    public record DepositToPortfolioRequest {

        public Guid PortfolioId { get; set; }
        public double Deposit { get; set; }
    }
}
