namespace ClientAPI.Models.PortfolioEvents {
    public record InvestmentDeposit {

        public string InvestmentId { get; set; }
        public double DepositAmount { get; set; }
    }
}
