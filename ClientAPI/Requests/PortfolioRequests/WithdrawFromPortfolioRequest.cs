using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientAPI.Requests.PortfolioRequests {
    public record WithdrawFromPortfolioRequest {

        public Guid PortfolioId { get; set; }
        public double Withdrawal { get; set; }
    }
}
