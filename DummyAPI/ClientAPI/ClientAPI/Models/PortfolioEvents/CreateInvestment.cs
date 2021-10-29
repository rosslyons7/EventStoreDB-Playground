using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientAPI.Models.PortfolioEvents {
    public record CreateInvestment {

        public string InvestmentId { get; set; }
        public double InitialInvestment { get; set; }
    }
}
