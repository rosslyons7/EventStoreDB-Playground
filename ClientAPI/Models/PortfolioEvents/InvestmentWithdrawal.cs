namespace ClientAPI.Models.PortfolioEvents {
    public record InvestmentWithdrawal {

        public string InvestmentId { get; set; }
        public double WithdrawalAmount { get; set; }
    }
}
