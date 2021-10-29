using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientAPI.Models.PortfolioEvents {
    public record ChangePrice {

        public string InvestmentId { get; set; }
        public double PercentageChange { get; set; }
    }
}
