namespace ClientAPI.Models.PortfolioEvents {
    public record ChangePrice {

        public string InvestmentId { get; set; }
        public double PercentageChange { get; set; }
    }
}
