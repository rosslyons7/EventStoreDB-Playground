namespace ClientAPI.Models.PortfolioEvents {
    public record CreateInvestment {

        public string InvestmentId { get; set; }
        public double InitialInvestment { get; set; }
    }
}
