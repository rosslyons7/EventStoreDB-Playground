using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientAPI.Models.PortfolioEvents {
    public class InvestmentWithdrawal {

        public string InvestmentId { get; set; }
        public double WithdrawalAmount { get; set; }
    }
}
