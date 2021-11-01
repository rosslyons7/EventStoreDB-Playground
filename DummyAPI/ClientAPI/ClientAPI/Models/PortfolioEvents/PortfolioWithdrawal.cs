using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientAPI.Models.PortfolioEvents {
    public record PortfolioWithdrawal {

        public double WithdrawalAmount { get; set; }
    }
}
