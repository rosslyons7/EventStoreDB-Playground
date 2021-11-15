using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientAPI.Models.PortfolioEvents {
    public record InvestmentDeposit {

        public string InvestmentId { get; set; }
        public double DepositAmount { get; set; }
    }
}
